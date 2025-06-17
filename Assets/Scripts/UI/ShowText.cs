using WarBall.UI.Pool;
using UnityEngine;

namespace WarBall.Common
{
    public class ShowText : MonoBehaviour
    {
        private static Transform _parent;

        private void Start()
        {
            _parent = transform;
        }

        public static UIPool Set(Vector2 pos,string text)
        {   
            UIPool ui = UIPoolManager.Release(UIPoolPrefab.ValText, _parent);
            ui.RectTransform.anchoredPosition = pos;
            ui.TMP.text = text;

            return ui;
        }
    }
}
