using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using WarBall.Events.Game;
using WarBall.UI.Base;
using WarBall.XML;
using WarBall.Grid.Base;

namespace WarBall.UI.Rest
{
    public class UICheckPanel : UIBase
    {
        private Canvas _canvas;

        private string _id;

        [FoldoutGroup("UI组件")][LabelText("名称")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _nameTMP;
        [FoldoutGroup("UI组件")][LabelText("背景图")][SuffixLabel("Image")][SerializeField] private Image _bcg;
        [FoldoutGroup("UI组件")][LabelText("图标")][SuffixLabel("Image")][SerializeField] private Image _icon;
        [FoldoutGroup("UI组件")][LabelText("描述")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _descriptionTMP;
        [FoldoutGroup("UI组件")][LabelText("分数")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _scoreTMP;
        [FoldoutGroup("UI组件")][LabelText("笑话")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _jokeTMP;
        [FoldoutGroup("UI组件")][LabelText("取消按钮")][SuffixLabel("Button")][SerializeField] private Button _cancelBtn;
        [FoldoutGroup("UI组件")][LabelText("丢弃按钮")][SuffixLabel("Button")][SerializeField] private Button _abandonBtn;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _cancelBtn.onClick.AddListener(() =>
            {
                OnCheckPanel(false, "");
            });

            _abandonBtn.onClick.AddListener(() =>
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.AbandonGrid, _id);
                OnCheckPanel(false, "");
            });
        }

        private void OnEnable()
        {
            GameEvents.Instance.RegisterEvent(EGameEventType.UICheckPanel, (Action<bool, string>)OnCheckPanel);
        }

        private void OnDisable()
        {
            GameEvents.Instance.UnregisterEvent(EGameEventType.UICheckPanel, (Action<bool, string>)OnCheckPanel);
        }

        private void OnCheckPanel(bool active,string id)
        {
            _canvas.enabled = active;

            if (!active)
            {
                _id = null;
                return;
            }

            _id = id;
            GridBase data = GridList.Instance.CheckData(_id);

            _nameTMP.text = data.Name;
            _bcg.sprite = GridList.Instance.GetSprite(_id).Bcg;
            _icon.sprite = GridList.Instance.GetSprite(_id).Icon;
            _descriptionTMP.text = data.Description;
            _scoreTMP.text = data.Score.ToString();
            _jokeTMP.text = data.Joke;

        }
    }
}
