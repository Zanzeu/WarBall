using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WarBall.UI.Game
{
    public class UIGridState : MonoBehaviour
    {
        private Image _icon;
        private TextMeshProUGUI _valTMP;

        public void Init(Sprite icon) => OnInit(icon);

        private void OnInit(Sprite icon)
        {   
            if (_icon == null)
            {
                _icon = GetComponentInChildren<Image>();
                _valTMP = GetComponentInChildren<TextMeshProUGUI>();
            }
            _icon.sprite = icon;
        }

        public void SetText(int val)
        {
            _valTMP.text = val.ToString();
        }
    }
}
