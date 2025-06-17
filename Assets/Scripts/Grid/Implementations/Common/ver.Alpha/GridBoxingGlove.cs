using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridBoxingGlove : GridBase, IGridCollisionHandle
    {
        [XmlElement("DMGValue")]
        public float DMGValue { get; set; }

        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, "DMG", Game.EAttributeOperation.SetTemp, DMGValue);
        }
    }
}
