using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridLottery : GridBase, IGridCollisionHandle
    {
        [XmlElement("Price")]
        public int Price { get; set; }

        public void OnCollisionHandle()
        {
            if (GameProcess.Instance.Score <= 0)
            {
                return;
            }
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateScore, -Price);
            Score = GetValue();
        }

        private int GetValue()
        {
            int random = UnityEngine.Random.Range(0, 100);

            if (random < 1) 
            {
                return 100000;
            }
            else if (random < 5)
            {
                return 2000;
            }
            else if (random < 15)
            {
                return 1000;
            }
            else if (random < 30)
            {
                return 200;
            }
            else
            {
                return 0;
            }
        }
    }
}
