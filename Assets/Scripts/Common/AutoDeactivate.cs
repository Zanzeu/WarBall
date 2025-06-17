using UnityEngine;

namespace WarBall.Common
{
    public class AutoDeactivate : MonoBehaviour
    {
        [SerializeField] private float lifetime;
        private float timer;

        private void Update()
        {
            if (Time.time - timer > lifetime)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            timer = Time.time;
        }
    }
}