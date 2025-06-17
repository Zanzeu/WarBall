using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridGlowdust : GridBase, IGridCollisionHandle
    {
        [XmlElement("COOLValue")]
        public float COOLValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "COOL", Game.EAttributeOperation.SetTemp, COOLValue);
        }
    }
}