using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using WarBall.Events.Game;
using WarBall.Grid.Base;
using WarBall.XML;

namespace WarBall.UI.Rest
{
    public class UIStore : MonoBehaviour
    {
        [FoldoutGroup("物品")][LabelText("选项")][SerializeField] private List<UIStoreItem> items = new List<UIStoreItem>();

        private List<string> _temp;

        private void Awake()
        {
            _temp = new List<string>();
        }

        private void OnEnable()
        {
            GameEvents.Instance.RegisterEvent(EGameEventType.UpdateStore,(Action)OnUpdateStore);
        }

        private void OnDisable()
        {
            GameEvents.Instance.UnregisterEvent(EGameEventType.UpdateStore, (Action)OnUpdateStore);
        }

        private void OnUpdateStore()
        {
            _temp.Clear();

            foreach (var item in items)
            {
                GridBase data = null;
                string id = "";
                do
                {
                    id = GridList.Instance.RandomGetGamingId();
                } while (_temp.Contains(id));
                _temp.Add(id);
                data = GridList.Instance.CheckData(id);
                item.OnUpdateItem(data);
                item.gameObject.SetActive(true);
            }
        }
    }
}
