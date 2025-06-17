using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using WarBall.Common;
using WarBall.XML;
using WarBall.Events.Game;

namespace WarBall.Entity.Other
{
    public class RainCloud : MonoBehaviour, IEntity
    {
        [FoldoutGroup("基础设置")][LabelText("飞行速度")][SerializeField] private float initSpeed;
        [FoldoutGroup("基础设置")][LabelText("移动速度")][SerializeField] private float moveSpeed;
        [FoldoutGroup("基础设置")][LabelText("下雨间隔")][SerializeField] private float dropInterval;

        private float timer;

        private Vector2 _direction;

        private Vector2[] _dir = { Vector2.left, Vector2.right };

        private bool _init = false;
        private bool _reach = false;

        private void OnEnable()
        {
            if (_init)
            {
                MoveToTarget();
            }
            else
            {
                _init = true;
            }
        }

        private void OnDisable()
        {
            _reach = false;
        }

        private void Update()
        {
            if (_reach)
            {
                transform.Translate(_direction * moveSpeed * Time.deltaTime);
                if (transform.position.x >= 8f)
                {
                    _direction = Vector2.left;
                }
                else if (transform.position.x <= -18f)
                {
                    _direction = Vector2.right;
                }

                if (Time.time - timer >= dropInterval)
                {
                    GameObject obj = PoolManager.Release(PrefabList.Instance.GetPrefab("water_drop"), new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y));
                    GameEvents.Instance.TriggerEvent(EGameEventType.EntitySpawn, obj);
                    timer = Time.time;
                }
            }
        }

        private void MoveToTarget()
        {
            float x = Random.Range(-18f, 8f);
            transform.DOMove(new Vector2(x, 13f), initSpeed).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                timer = Time.time;
                _direction = _dir[Random.Range(0, 2)];
                _reach = true;
            });
        }
    }
}
