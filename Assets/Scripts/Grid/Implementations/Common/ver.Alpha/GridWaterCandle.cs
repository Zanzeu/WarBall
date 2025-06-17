using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridWaterCandle : GridBase, IGridCollisionHandle
    {
        [XmlElement("SPAWNValue")]
        public float SPAWNValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "SPAWN", Game.EAttributeOperation.SetTemp, SPAWNValue);
        }
    }
}
