using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WarBall.Common;

namespace WarBall.Persistent
{   
    public enum EGlobalGameEventType
    {
        TimerSoundEnd,
    }

    public class GlobalEventsManager : PersistentSingleton<GlobalEventsManager>
    {
        private Dictionary<EGlobalGameEventType, Delegate> _eventHandlers;

        protected override void Awake()
        {
            base.Awake();
            _eventHandlers = new Dictionary<EGlobalGameEventType, Delegate>();
        }

        public void RegisterEvent(EGlobalGameEventType eventType, Delegate handler)
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

        public void UnregisterEvent(EGlobalGameEventType eventType, Delegate handler)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = Delegate.Remove(_eventHandlers[eventType], handler);
            }
        }

        public void TriggerEvent(EGlobalGameEventType eventType, params object[] args)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler))
            {
                handler?.DynamicInvoke(args);
            }
        }

        public void TriggerEvent(EGlobalGameEventType eventType)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action action)
            {
                action?.Invoke();
            }
        }

        public void TriggerEvent<T>(EGlobalGameEventType eventType, T arg)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action<T> action)
            {
                action?.Invoke(arg);
            }
        }

        public void TriggerEvent<T1, T2>(EGlobalGameEventType eventType, T1 arg1, T2 arg2)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action<T1, T2> action)
            {
                action?.Invoke(arg1, arg2);
            }
        }

        public void TriggerEvent<T1, T2, T3>(EGlobalGameEventType eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            if (_eventHandlers.TryGetValue(eventType, out var handler) && handler is Action<T1, T2, T3> action)
            {
                action?.Invoke(arg1, arg2, arg3);
            }
        }
    }
}
