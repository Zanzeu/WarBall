using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridAddScoreMachine : GridBase, IGridCollisionHandle
    {
        [XmlElement("SCOREValue")]
        public float SCOREValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "SCORE", Game.EAttributeOperation.SetTemp, SCOREValue);
        }
    }
}
