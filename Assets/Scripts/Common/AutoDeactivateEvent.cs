using UnityEngine;
using WarBall.Events.Game;

namespace WarBall.Common
{
    public class AutoDeactivateEvent : MonoBehaviour
    {
        [SerializeField] private float lifetime;
        private float timer;

        private void Update()
        {
            if (Time.time - timer > lifetime)
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.EntityDeath, gameObject);
            }
        }

        private void OnEnable()
        {
            timer = Time.time;
        }
    }
}
