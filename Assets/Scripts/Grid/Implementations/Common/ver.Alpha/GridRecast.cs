using System;
using System.Xml.Serialization;
using UnityEngine;
using WarBall.Events.Game;
using WarBall.Grid.Base;
using WarBall.Util;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridRecast : GridBase, IGridCollisionHandle
    {
        [XmlElement("MaxVal")]
        public float MaxVal { get; set; }

        [XmlElement("MinVal")]
        public float MinVal { get; set; }

        public void OnCollisionHandle()
        {
            string attributeName = GetRandomAttributes.GetName();
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, attributeName
                , Game.EAttributeOperation.SetBase, GetRandomAttributes.GetValue(attributeName, MinVal, MaxVal));
        }
    }
}
