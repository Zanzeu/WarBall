using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Sirenix.OdinInspector;
using WarBall.Events.Game;
using WarBall.Grid.Base;
using WarBall.XML;

namespace WarBall.UI.Rest
{
    public class UIStoreItem : MonoBehaviour, IPointerDownHandler
    {
        private bool _buy = true;
        private GridBase _data;

        [FoldoutGroup("UI组件")][LabelText("背景")][SuffixLabel("Image")][SerializeField] private Image bcg;
        [FoldoutGroup("UI组件")][LabelText("图标")][SuffixLabel("Image")][SerializeField] private Image icon;
        [FoldoutGroup("UI组件")][LabelText("名称")][SuffixLabel("TMP")][SerializeField] private new TextMeshProUGUI name;
        [FoldoutGroup("UI组件")][LabelText("描述")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI description;
        [FoldoutGroup("UI组件")][LabelText("遮罩")][SuffixLabel("Image")][SerializeField] private Image mask;

        private void OnEnable()
        {
            GameEvents.Instance.RegisterEvent(EGameEventType.BuyItem, (Action<bool>)OnButItem);
        }

        private void OnDisable()
        {
            GameEvents.Instance.UnregisterEvent(EGameEventType.BuyItem, (Action<bool>)OnButItem);
        }

        public void OnPointerDown(PointerEventData eventData)
        {   
            if (_buy)
            {
                return;
            }

            bool success = GameEvents.Instance.TriggerEvent<string, bool>(EGameEventType.GetGrid, _data.ID);
            if (success)
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.BuyItem, true);
                _data = null;
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Full");
            }
        }

        public void OnUpdateItem(GridBase data)
        {
            OnButItem(false);

            _data = data;
            bcg.sprite = GridList.Instance.GetSprite(_data.ID).Bcg;
            icon.sprite = GridList.Instance.GetSprite(_data.ID).Icon;
            name.text = _data.Name;
            description.text = _data.Description;
        }

        private void OnButItem(bool buy)
        {
            _buy = buy;
            mask.enabled = _buy;
            if (_buy)
            {
                
            }
            else
            {

            }
        }
    }
}
