using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridMedicalKit : GridBase, IGridCollisionHandle
    {
        [XmlElement("RecoverValue")]
        public float RecoverValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "HP", Game.EAttributeOperation.SetCur, RecoverValue);
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateHP, true);
        }
    }
}
