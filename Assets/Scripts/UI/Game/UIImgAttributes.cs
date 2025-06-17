using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using WarBall.Ball.Base;
using WarBall.Game;
using WarBall.UI.Base;
using WarBall.Events.Game;
using System;

namespace WarBall.UI.Game
{
    public class UIImgAttributes : UIBase
    {
        private Image _hpFill;

        private BallAttributes _attributes;

        private void Start()
        {
            _hpFill = transform.GetChild(0).GetChild(0).GetComponent<Image>();

            _attributes = UIManager.GetUI<UIAttributes>("attributes").GetComponent<BallAttributes>();
        }

        private void OnEnable()
        {
            GameEvents.Instance.RegisterEvent(EGameEventType.UpdateHP, (Action<bool>)OnUpdateHP);
        }

        private void OnDisable()
        {
            GameEvents.Instance.UnregisterEvent(EGameEventType.UpdateHP, (Action<bool>)OnUpdateHP);
        }

        public void OnUpdateHP(bool tween = true)
        {
            if (tween)
            {
                _hpFill.DOFillAmount(_attributes.Attributes["HP"].CurValue / _attributes.Attributes["HP"].BaseValue, 0.2f);
            }
            else
            {
                _hpFill.fillAmount = _attributes.Attributes["HP"].CurValue / _attributes.Attributes["HP"].BaseValue;
            }
        }
    }
}