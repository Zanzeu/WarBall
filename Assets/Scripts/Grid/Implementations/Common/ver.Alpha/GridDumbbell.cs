using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridDumbbell : GridBase, IGridCollisionHandle
    {
        [XmlElement("HPValue")]
        public float HPValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "HP", Game.EAttributeOperation.SetMax, HPValue);
        }
    }
}
