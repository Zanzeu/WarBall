using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WarBall.Events.Game;
using WarBall.UI.Base;

namespace WarBall.UI.Game
{
    public class UISwitchButton : UIBase
    {
        private Button _switchBtn;

        private GameObject _attributes;
        private GameObject _effect;

        private bool _checkAttributes;

        protected void Start()
        {
            _switchBtn = GetComponent<Button>();

            _attributes = UIManager.GetUI<UIAttributes>("attributes").transform.GetChild(0).gameObject;
            _effect = UIManager.GetUI<UIEffect>("effect").gameObject;

            _attributes.SetActive(false);
            _effect.SetActive(true);

            _checkAttributes = false;

            _switchBtn.onClick.AddListener(OnSwitchPanel);
        }


        private void OnSwitchPanel()
        {
            _checkAttributes = !_checkAttributes;
            _attributes.SetActive(_checkAttributes);
            _effect.SetActive(!_checkAttributes);

            GameEvents.Instance.TriggerEvent(EGameEventType.ClickSwitchPanelButton, _checkAttributes);
        }
    }
}
