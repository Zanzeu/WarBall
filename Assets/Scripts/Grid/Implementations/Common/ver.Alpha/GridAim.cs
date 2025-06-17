using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridAim : GridBase, IGridCollisionHandle
    {
        [XmlElement("CRTRValue")]
        public float CRTRValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "CRTR", Game.EAttributeOperation.SetTemp, CRTRValue);
        }
    }
}
