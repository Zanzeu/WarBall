using DG.Tweening;
using UnityEngine;
using WarBall.Common;
using WarBall.Config;
using WarBall.Enemy;
using WarBall.Enemy.Base;
using WarBall.Events.Game;
using WarBall.Game;
using WarBall.Map;
using WarBall.Persistent;
using WarBall.XML;

namespace WarBall.Agent
{
    public class EnemyAgent : MonoBehaviour, IAgent, ILifeDeath , IPririority
    {
        public EnemyBase Data { get; private set; }

        protected EnemyAttributes _attributes;
        private Sequence _scaleSequence;
        private SpriteRenderer _spriteBcgRenderer;
        private BoxCollider2D _boxCollider;
        private GameObject _deathVFX;
        public SpriteRenderer _spriteRenderer { get; set; }

        public EPriority Priority => EPriority.Low;

        private float _lifeTimer;
        private Vector2Int _index;
        private bool _canCollision;
        private float _attributeLift;
        private float _lift;
        private bool _init = false;

        public void Set(Vector2Int pos, EnemyBase data) => OnSet(pos, data);
        public float Collision(float damage) => OnCollision(damage);
        public void Died() => OnDied();
        public void Retreat() => OnRetreat();

        private void Update()
        {   
            if (!_init)
            {
                return;
            }

            if (Time.time - _lifeTimer > _lift)
            {
                Retreat();
            }
        }

        private void OnSet(Vector2Int pos, EnemyBase data)
        {
            if (_spriteBcgRenderer == null)
            {
                _spriteBcgRenderer = GetComponentInChildren<SpriteRenderer>();
                _spriteRenderer = _spriteBcgRenderer.transform.Find("sprite").GetComponent<SpriteRenderer>();
                _deathVFX = PrefabList.Instance.GetPrefab("vfx", "VFXEnemyDeathPrefab");
                _boxCollider = GetComponent<BoxCollider2D>();
            }
            _canCollision = true;
            Data = data;

            _spriteRenderer.sprite = EnemyList.Instance.GetSprite(Data.ID).Icon;
            _spriteBcgRenderer.sprite = EnemyList.Instance.GetSprite(Data.ID).Bcg;
            _index = pos;
            _attributes = new EnemyAttributes(Data.HP, Data.ATK, Data.DEF,  Data.SCORE, Data.COOL, Data.LIFE);
            _attributeLift = _attributes.Attributes["LIFE"].ApplyValue;
            _lift = Random.Range(_attributeLift - 2f, _attributeLift + 2f);
            _lifeTimer = Time.time;
            _init = true;
            transform.DOScale(new Vector3(1, 1, 1), 0.5f).OnComplete(() =>
            {
                _boxCollider.enabled = true;
            });
        }
        private float OnCollision(float damage)
        {   
            if (!_canCollision)
            {
                return 0f;
            }
            _canCollision = false;
            DoScale();
            OnHurt(damage);

            return _attributes.Attributes["ATK"].ApplyValue;
        }
        private void OnHurt(float damage)
        {
            AttributeForge forge = _attributes.SetAttributes("HP", EAttributeOperation.SetCur, -ActualDamage(damage));
            if (forge.CurValue <= 0f)
            {
                OnDied();
            }
        }
        private void OnRetreat()
        {
            _boxCollider.enabled = false;
            transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f).OnComplete(() =>
            {
                KillTween();
                OnDeath();
            });
        }
        private void OnDied()
        {   
            if (Data is IEnemyDeathHandle grid)
            {
                grid.OnDeath();
            }
            _boxCollider.enabled = false;
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            IncreaseScore();
            KillTween();
            OnDeath();

            AudioManager.Instance.PlaySound(Sounds.EnemyDeath);
            PoolManager.Release(_deathVFX, transform.position);
        }
        public void OnDeath()
        {
            KillTween();
            GameMap.Instance.ReturnMap(_index);
            _index = -Vector2Int.one;
            Data = null;
            _init = false;
            gameObject.SetActive(false);
        }

        private void KillTween()
        {
            _scaleSequence?.Kill();
        }
        private void DoScale()
        {
            if (_scaleSequence != null && _scaleSequence.IsActive())
            {
                return;
            }

            _scaleSequence = DOTween.Sequence();
            _scaleSequence.Append(_spriteBcgRenderer.transform.DOScale(new Vector2(GlobalGridConfig.VALUE_TWEEN_SCALE, GlobalGridConfig.VALUE_TWEEN_SCALE), GlobalGridConfig.TIME_SCALE).SetEase(Ease.OutQuad));
            _scaleSequence.Append(_spriteBcgRenderer.transform.DOScale(Vector2.one, GlobalGridConfig.TIME_SCALE).SetEase(Ease.InQuad));
            _scaleSequence.OnComplete(() =>
            {   
                if (Data is IEnemyDoScaleCompleteHandle e)
                {
                    e.OnScaleComplete();
                }

                if (Data is IEnemySwitchPositionHandle grid2)
                {
                    grid2.OnSwitchPosition();
                    OnSwitchPosition();
                }

                _canCollision = true;
            });
            _scaleSequence.Play();
        }
        private void IncreaseScore()
        {
            if (Data.SCORE > 0)
            {
                PoolManager.Release(PrefabList.Instance.GetPrefab("addScoreTextPrefab"), transform.position).GetComponent<ShowScoreValue>().Set(transform.position.y + 1.5f,(int)Data.SCORE);
            }
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateScore, (int)_attributes.Attributes["SCORE"].ApplyValue);
        }
        private float ActualDamage(float damage)
        {
            float DEF = _attributes.Attributes["DEF"].ApplyValue;
            float effectiveDEF = Mathf.Max(DEF, 0f);
            float damageMultiplier = 100f / (effectiveDEF + 100f);
            float actualDamage = damage * damageMultiplier;
            return actualDamage;
        }

        public void Hurt(float damage)
        {
            OnHurt(damage);
        }

        private void OnSwitchPosition()
        {
            Vector2Int? pos = GameMap.Instance.GetTile(out Vector2Int index);
            if (pos == null || index.x < 0)
            {
                return;
            }
            GameMap.Instance.ReturnMap(_index);
            _index = index;
            transform.position = (Vector2)pos;
        }
    }
}
