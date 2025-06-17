using UnityEngine;
using Sirenix.OdinInspector;
using WarBall.Agent;
using WarBall.Ball.Base;
using WarBall.Events.Game;

namespace WarBall.Entity.Car
{
    public class CarBase : MonoBehaviour, IEntity
    {
        [FoldoutGroup("��������")][LabelText("�˺�����")][SerializeField] private float damageRate;
        [FoldoutGroup("��������")][LabelText("�ƶ��ٶ�")][SerializeField] private float moveSpeed;

        protected virtual void Update()
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyAgent>().Collision(damageRate * BallAttributes.Instance.Attributes["ATK"].ApplyValue);
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(-360f, 360f));
            }
            else if (collision.CompareTag("Obstacle"))
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.EntityDeath, gameObject);
            }
        }
    }
}
