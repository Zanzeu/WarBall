using UnityEngine;
using WarBall.Agent;
using WarBall.Config;
using WarBall.Persistent;

namespace WarBall.Entity.Ball
{
    [RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
    public class BallEntityBase : MonoBehaviour,IEntity
    {
        private Rigidbody2D _rb;
        private Vector3 _moveDir;
        private int _layerMask;
        private float _currentSpeed;

        private int[] _direction = { -1, 1 };

        public void Set(Vector2 direction) => OnSet(direction);
        public void SetDirection(Vector2 direcion) => _moveDir = direcion;

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentGameStatus != EGameStatus.Active)
            {
                return;
            }

            Movement();
            CheckCollision();
        }

        private void OnSet(Vector2 direction)
        {   
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody2D>();
                _layerMask = LayerMask.GetMask("Grid", "Obstacle", "Enemy");
            }
            
            _currentSpeed = 20f;

            SetDirection(direction);
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

                HandleCollision(hit);
            }
        }

        private void HandleCollision(RaycastHit2D hit)
        {
            Vector2 reflection = Vector2.Reflect(_moveDir, hit.normal);
            _moveDir = reflection.normalized;

            if (Mathf.Abs(_moveDir.x) <= 0.05f)
            {
                _moveDir.x = _direction[Random.Range(0, 2)] * 0.05f;
            }

            if (Mathf.Abs(_moveDir.y) <= 0.05f)
            {
                _moveDir.y = _direction[Random.Range(0, 2)] * 0.05f;
            }

            _moveDir = _moveDir.normalized;

            _currentSpeed += 2f * Time.fixedDeltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, GlobalGridConfig.MIN_SPEED, GlobalGridConfig.MAX_SPEED);
            _rb.velocity = _moveDir * _currentSpeed;

            //AudioManager.Instance.PlaySound(Sounds.BallBounce);
        }

        public void SetSpeed(float speed)
        {
            _currentSpeed = speed;
        }
    }
}
