using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WarBall.UI.Base;
using WarBall.Ball.Base;
using WarBall.Game;
using WarBall.Events.Game;
using System;
using WarBall.Common;

namespace WarBall.UI.Game
{
    public class UIAttributes : UIBase
    {
        private BallAttributes _ballAttributes;
        private Dictionary<string, TextMeshProUGUI> _TMPs;
        private bool _showText = false;

        protected override void Awake()
        {
            base.Awake();
            _TMPs = new Dictionary<string, TextMeshProUGUI>();
        }

        protected void Start()
        {
            _ballAttributes = GetComponent<BallAttributes>();

            foreach (Transform child in transform.GetChild(0))
            {
                _TMPs[child.name] = child.GetChild(1).GetComponent<TextMeshProUGUI>();
            }

            AttributesInit();
        }

        private void OnEnable()
        {
            GameEvents.Instance.RegisterEvent(EGameEventType.UpdateAttributes, (Action<string, EAttributeOperation, float>)OnUpdateAttributes);
            GameEvents.Instance.RegisterEvent(EGameEventType.ResetAttributes, (Action<string>)OnResetAttributes);
            GameEvents.Instance.RegisterEvent(EGameEventType.ClickSwitchPanelButton, (Action<bool>)OnClickSwitchPanelButton);
        }

        private void OnDisable()
        {
            GameEvents.Instance.UnregisterEvent(EGameEventType.UpdateAttributes, (Action<string, EAttributeOperation, float>)OnUpdateAttributes);
            GameEvents.Instance.UnregisterEvent(EGameEventType.ResetAttributes, (Action<string>)OnResetAttributes);
            GameEvents.Instance.UnregisterEvent(EGameEventType.ClickSwitchPanelButton, (Action<bool>)OnClickSwitchPanelButton);
        }

        private void AttributesInit()
        {
            Dictionary<string, AttributeForge> attributes = _ballAttributes.GetAllAttribute();
            foreach (var attribute in attributes.Values)
            {
                SetText(attribute.Name, attribute);
            }
        }

        private void OnUpdateAttributes(string attribute, EAttributeOperation target, float val)
        {
            AttributeForge forge = _ballAttributes.SetAttributes(attribute, target, val);
            SetText(attribute, forge);

            if (_showText)
            {
                ShowText.Set(GetAttributesPos(attribute, val, out string text), text);
            }
        }

        private void OnResetAttributes(string attribute)
        {
            SetText(attribute, _ballAttributes.Attributes[attribute]);
        }

        public void SetText(string name,AttributeForge forge)
        {
            string formattedApplyValue = "";

            if (!forge.Percentage)
            {   
                formattedApplyValue = forge.ApplyValue.ToString();
            }
            else
            {
                formattedApplyValue = forge.ApplyValue.ToString("P0");
            }

            _TMPs[name].text = $"{formattedApplyValue}";
        }

        private Vector2 GetAttributesPos(string name, float val ,out string text)
        {   
            switch(name)
            {
                case "ATK":
                    text = val > 0 ? $"+{(int)val}" : $"{(int)val}";
                    return new Vector2(820f, 230f);
                case "DEF":
                    text = val > 0 ? $"+{(int)val}" : $"{(int)val}";
                    return new Vector2(820f, 150f);
                case "CRTR":
                    text = val > 0 ? $"+{val.ToString("P0")}" : $"{val.ToString("P0")}";
                    return new Vector2(820f, 85f);
                case "DMG":
                    text = val > 0 ? $"+{val.ToString("P0")}" : $"{val.ToString("P0")}";
                    return new Vector2(820f, 13f);
                case "SCORE":
                    text = val > 0 ? $"+{val.ToString("P0")}" : $"{val.ToString("P0")}";
                    return new Vector2(820f, -55f);
                case "COOL":
                    text = val > 0 ? $"{val.ToString("P0")}" : $"+{val.ToString("P0")}";
                    return new Vector2(820f, -125f);
                case "SPAWN":
                    text = val > 0 ? $"+{val.ToString("P0")}" : $"{val.ToString("P0")}";
                    return new Vector2(820f, -200f);
            }

            text = "";
            return Vector2.zero;
        }

        private void OnClickSwitchPanelButton(bool trigger)
        {
            _showText = trigger;
        }
    }
}
