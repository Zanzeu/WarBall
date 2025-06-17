using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using WarBall.Common;
using WarBall.XML;

namespace WarBall.Persistent
{
    public enum EGameStatus
    {
        Menu,
        Prepare,
        Active,
        Pause,
        End,
        Loading,
        Rest,
    }

    public class GameManager : PersistentSingleton<GameManager>
    {
        [LabelText("当前球ID")] public string BallID;
        [LabelText("当前状态")][ReadOnly][ShowInInspector] public EGameStatus CurrentGameStatus { private set; get; }
        private Dictionary<EGameStatus, Delegate> _eventHandlers;

        protected override void Awake()
        {
            base.Awake();
            _eventHandlers = new Dictionary<EGameStatus, Delegate>();
            GridList.Instance.Init();
            BallList.Instance.Init();
            EnemyList.Instance.Init();
            PrefabList.Instance.Init();
        }

        public void RegisterEvent(EGameStatus eventType, Delegate handler)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = Delegate.Combine(_eventHandlers[eventType], handler);
            }
            else
            {
                _eventHandlers.Add(eventType, handler);
            }
        }

        public void UnregisterEvent(EGameStatus eventType, Delegate handler)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = Delegate.Remove(_eventHandlers[eventType], handler);
            }
        }
        public void SwitchGameStatus(EGameStatus status, bool triggerEvent = true)
        {
            CurrentGameStatus = status;
            if (triggerEvent)
            {
                TriggerEvent(status);
            }
        }

        private void TriggerEvent(EGameStatus eventType)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action action)
            {
                action?.Invoke();
            }
        }
    }
}
