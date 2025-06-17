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

        [FoldoutGroup("UI���")][LabelText("����")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _nameTMP;
        [FoldoutGroup("UI���")][LabelText("����ͼ")][SuffixLabel("Image")][SerializeField] private Image _bcg;
        [FoldoutGroup("UI���")][LabelText("ͼ��")][SuffixLabel("Image")][SerializeField] private Image _icon;
        [FoldoutGroup("UI���")][LabelText("����")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _descriptionTMP;
        [FoldoutGroup("UI���")][LabelText("����")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _scoreTMP;
        [FoldoutGroup("UI���")][LabelText("Ц��")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _jokeTMP;
        [FoldoutGroup("UI���")][LabelText("ȡ����ť")][SuffixLabel("Button")][SerializeField] private Button _cancelBtn;
        [FoldoutGroup("UI���")][LabelText("������ť")][SuffixLabel("Button")][SerializeField] private Button _abandonBtn;

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
