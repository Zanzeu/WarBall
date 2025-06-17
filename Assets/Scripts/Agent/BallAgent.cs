using UnityEngine;
using WarBall.Ball.Base;
using WarBall.Config;
using WarBall.Events.Game;
using WarBall.Persistent;
using WarBall.UI.Base;
using WarBall.UI.Game;
using WarBall.XML;

namespace WarBall.Agent
{
    [RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
    public class BallAgent : MonoBehaviour, IAgent
    {
        private BallBase _data;
        public SpriteRenderer SpriteRenderer { get; set; }

        private SpriteRenderer _spriteBcgRenderer;
        private Rigidbody2D _rb;
        private Vector3 _moveDir;
        private int _layerMask;
        private float _currentSpeed;
        private BallAttributes _attributes;

        public void Set(BallBase data, Vector2 direction) => OnSet(data, direction);
        public void Death() => OnDeath();
        public void SetDirection(Vector2 direcion) => _moveDir = direcion;

        private int[] _direction = { -1, 1 };

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentGameStatus != EGameStatus.Active)
            {
                return;
            }

            Movement();
            CheckCollision();
        }

        private void OnSet(BallBase data, Vector2 direction)
        {
            if (_spriteBcgRenderer == null)
            {
                _rb = GetComponent<Rigidbody2D>();
                _spriteBcgRenderer = GetComponentInChildren<SpriteRenderer>();
                SpriteRenderer = _spriteBcgRenderer.transform.Find("sprite").GetComponent<SpriteRenderer>();
                _attributes = UIManager.GetUI<UIAttributes>("attributes").GetComponent<BallAttributes>();

                _layerMask = LayerMask.GetMask("Grid", "Obstacle", "Enemy");
            }

            _data = data;
            _currentSpeed = 20f;

            _data.OnInit(this);

            SpriteRenderer.sprite = BallList.Instance.GetIcon(_data.ID);

            SetDirection(direction);
        }
        private void OnDeath()
        {
            _currentSpeed = 0f;
            _moveDir = Vector3.zero;
            SpriteRenderer.sprite = null;
            _data = null;

            gameObject.SetActive(false);
        }

        private void Movement()
        {
            _rb.velocity = _moveDir * _currentSpeed;
        }

        private void CheckCollision()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, _moveDir, 0.1f, _layerMask);

            if (hit)
            {
                if (hit.collider.CompareTag("Grid"))
                {
                    hit.collider.GetComponent<GridAgent>().Collision();
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    float atk = _attributes.Attributes["ATK"].ApplyValue;
                    float ballDamage = CheckCRIT() ? (atk * _attributes.Attributes["DMG"].ApplyValue) : atk;
                    float damage = hit.collider.GetComponent<EnemyAgent>().Collision(ballDamage);
                    GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "HP", Game.EAttributeOperation.SetCur, -ActualDamage(damage));
                    GameEvents.Instance.TriggerEvent(EGameEventType.UpdateHP, true);
                }

                if (_attributes.Attributes["HP"].CurValue <= 0f)
                {
                    GameManager.Instance.SwitchGameStatus(EGameStatus.End);
                    return;
                }

                HandleCollision(hit);
            }
        }

        private bool CheckCRIT()
        {
            float isCRIT = Random.Range(0f, 1f);

            return isCRIT <= _attributes.Attributes["CRTR"].ApplyValue;
        }

        private void HandleCollision(RaycastHit2D hit)
        {
            Vector2 reflection = Vector2.Reflect(_moveDir, hit.normal);
            _moveDir = reflection.normalized;

            _moveDir = _moveDir.normalized;

            _currentSpeed += 2f * Time.fixedDeltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, GlobalGridConfig.MIN_SPEED, GlobalGridConfig.MAX_SPEED);
            _rb.velocity = _moveDir * _currentSpeed;

            AudioManager.Instance.PlaySound(Sounds.BallBounce);
        }

        private float ActualDamage(float damage)
        {
            float DEF = _attributes.Attributes["DEF"].ApplyValue;
            float effectiveDEF = Mathf.Max(DEF, 0f);
            float damageMultiplier = 100f / (effectiveDEF + 100f);
            float actualDamage = damage * damageMultiplier;
            return actualDamage;
        }

        public void SetSpeed(float speed)
        {
            _currentSpeed = speed;
        }
    }
}
