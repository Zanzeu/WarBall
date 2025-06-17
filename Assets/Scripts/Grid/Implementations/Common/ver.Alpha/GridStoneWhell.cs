using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridStoneWhell : GridBase, IGridCollisionHandle
    {
        [XmlElement("ATKValue")]
        public float ATKValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "ATK", Game.EAttributeOperation.SetTemp, ATKValue);
        }
    }
}
