using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridBrick : GridBase, IGridSetUIHandle,IGridCollisionHandle,IGridTurnSettlement
    {
        [XmlElement("UIName")]
        public string UIName { get; set; }

        [XmlElement("ScoreRate")]
        public int ScoreRate { get; set; }

        private int _count;

        public void OnCollisionHandle()
        {
            _count = GameBlackboard.Instance.SetCount(UIName, 1);
        }

        public string OnSetUI(out int count)
        {
            count = _count;
            return UIName;
        }

        public void OnSettlement()
        {   
            if (GameBlackboard.Instance.CheckSettlement(UIName))
            {
                return;
            }
            _count = GameBlackboard.Instance.SetCount(UIName, 0);
            GameBlackboard.Instance.Settlement(UIName, true);

            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateScore, _count * ScoreRate);
            GameBlackboard.Instance.CloseUI(UIName);
        }
    }
}
