using UnityEngine;
using Sirenix.OdinInspector;
using WarBall.Agent;
using WarBall.Ball.Base;
using WarBall.Events.Game;

namespace WarBall.Entity.Bullet
{
    public class BulletBase : MonoBehaviour ,IEntity
    {
        [FoldoutGroup("基础设置")] [LabelText("伤害倍率")] [SerializeField] private float damageRate;
        [FoldoutGroup("基础设置")] [LabelText("飞行速度")] [SerializeField] protected float moveSpeed;

        protected virtual void Update()
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyAgent>().Collision(damageRate * BallAttributes.Instance.Attributes["ATK"].ApplyValue);
                gameObject.SetActive(false);
            }
            else if (collision.CompareTag("Obstacle"))
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.EntityDeath, gameObject);
            }
        }
    }
}
