using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WarBall.Events.Game;

namespace WarBall.UI.Rest
{
    public class UIBackpackCell : MonoBehaviour, IPointerDownHandler
    {
        private string _id;

        public void Set(string id)
        {
            _id = id;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UICheckPanel, true, _id);
        }
    }
}
