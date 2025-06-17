using UnityEngine;
using WarBall.UI.Pool;

namespace WarBall.Common
{
    public class AutoDeactivateUI : MonoBehaviour
    {
        [SerializeField] private float lifetime;
        private float timer;
        private UIPool ui;

        private void Start()
        {
            ui = GetComponent<UIPool>();
        }

        private void Update()
        {
            if (Time.time - timer > lifetime)
            {
                ui.Enterpool();
            }
        }

        private void OnEnable()
        {
            timer = Time.time;
        }
    }
}
