using System;
using System.Xml.Serialization;
using WarBall.Enemy.Base;
using WarBall.Events.Game;

namespace WarBall.Enemy.Implementations
{
    [Serializable]
    public class EnemySlime : EnemyBase, IEnemyDeathHandle
    {
        [XmlElement("SplitCount")]
        public int SplitCount { get; set; }

        public void OnDeath()
        {
            for (int i = 0; i < SplitCount; i++)
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.EnQueue, "enemy_slime_split", "High","Enemy");
            }
        }
    }
}
