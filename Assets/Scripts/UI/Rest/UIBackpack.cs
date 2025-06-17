using System;
using System.Collections;
using System.Collections.Generic;
using WarBall.UI.Pool;
using UnityEngine;
using WarBall.Events.Game;
using WarBall.Game;
using WarBall.Persistent;
using WarBall.XML;

namespace WarBall.UI.Rest
{
    public class UIBackpack : MonoBehaviour
    {
        private List<Transform> _cells;
        private List<UIPool> _temp;

        private void Awake()
        {
            _cells = new List<Transform>();
            _temp = new List<UIPool>();
        }

        private void Start()
        {
            foreach (Transform cell in transform)
            {
                _cells.Add(cell);
            }
        }

        private void OnEnable()
        {
            GameEvents.Instance.RegisterEvent(EGameEventType.UpdateBackpack, (Action)OnUpdateBackpack);
        }

        private void OnDisable()
        {
            GameEvents.Instance.UnregisterEvent(EGameEventType.UpdateBackpack, (Action)OnUpdateBackpack);
        }

        private void OnUpdateBackpack()
        {   
            foreach (var pool in _temp)
            {
                pool.Enterpool();
            }

            _temp.Clear();

            for (int i = 0; i < GameProcess.Instance.BackpackGrid.Count; i++)
            {
                UIPoolBackpackItem ui = (UIPoolBackpackItem)UIPoolManager.Release(UIPoolPrefab.BackpackItem, _cells[i]);
                ui.Icon.sprite = GridList.Instance.GetSprite(GameProcess.Instance.BackpackGrid[i]).Icon;
                ui.Bcg.sprite = GridList.Instance.GetSprite(GameProcess.Instance.BackpackGrid[i]).Bcg;
                ui.Cell.Set(GameProcess.Instance.BackpackGrid[i]);
                _temp.Add(ui);
            }
        }
    }
}
