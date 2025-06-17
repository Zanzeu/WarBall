using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

using WarBall.Common;
using WarBall.Game;
using WarBall.UI.Base;
using WarBall.UI.Game;
using WarBall.Ball.Base;
using WarBall.Persistent;

namespace WarBall.Events.Game
{   
    public enum EGameEventType
    {
        UpdateScore,
        UpdateAttributes,
        UpdateHP,
        UpdateStore,
        UpdateBackpack,
        UpdateToken,
        UpdateWave,

        UICheckPanel,

        ClickSwitchPanelButton,

        GetGrid,
        AbandonGrid,

        Spawn,
        EntitySpawn,
        EntityDeath,
        ResetAttributes,
        BuyItem,
        EnQueue,

        SetCurTurnTime,
        SetBallSpeed,

        GridDeath,
        EnemyDeath,
    }

    public class GameEvents : Singleton<GameEvents>
    {
        private Dictionary<EGameEventType, Delegate> _eventHandlers;

        protected override void Awake()
        {
            base.Awake();
            _eventHandlers = new Dictionary<EGameEventType, Delegate>();
            GameBlackboard.Instance.Init();
        }

        public void RegisterEvent(EGameEventType eventType, Delegate handler)
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

        private void Start()
        {
            GameManager.Instance.SwitchGameStatus(EGameStatus.Prepare, false);
        }

        public void UnregisterEvent(EGameEventType eventType, Delegate handler)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = Delegate.Remove(_eventHandlers[eventType], handler);
            }
        }

        public void TriggerEvent(EGameEventType eventType, params object[] args)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler))
            {
                handler?.DynamicInvoke(args);
            }
        }

        public void TriggerEvent(EGameEventType eventType)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action action)
            {
                action?.Invoke();
            }
        }

        public void TriggerEvent<T>(EGameEventType eventType, T arg)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action<T> action)
            {
                action?.Invoke(arg);
            }
        }

        public void TriggerEvent<T1, T2>(EGameEventType eventType, T1 arg1, T2 arg2)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action<T1, T2> action)
            {
                action?.Invoke(arg1, arg2);
            }
        }

        public void TriggerEvent<T1, T2, T3>(EGameEventType eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action<T1, T2, T3> action)
            {
                action?.Invoke(arg1, arg2, arg3);
            }
        }

        public T TriggerEvent<T>(EGameEventType eventType)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Func<T> action)
            {
                return action.Invoke();
            }

            return default;
        }

        public T2 TriggerEvent<T1, T2>(EGameEventType eventType, T1 arg)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Func<T1, T2> action)
            {
                return action.Invoke(arg);
            }

            return default;
        }

        public T3 TriggerEvent<T1, T2, T3>(EGameEventType eventType, T1 arg1, T2 arg2)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Func<T1, T2, T3> action)
            {
                return action.Invoke(arg1, arg2);
            }

            return default;
        }

        public T4 TriggerEvent<T1, T2,T3,T4>(EGameEventType eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Func<T1, T2, T3, T4> action) 
            {
                return action.Invoke(arg1, arg2, arg3);
            }

            return default;
        }
    }
}
