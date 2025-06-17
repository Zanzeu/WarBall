using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridArmor : GridBase, IGridCollisionHandle
    {
        [XmlElement("DEFValue")]
        public float DEFValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "DEF", Game.EAttributeOperation.SetTemp, DEFValue);
        }
    }
}
