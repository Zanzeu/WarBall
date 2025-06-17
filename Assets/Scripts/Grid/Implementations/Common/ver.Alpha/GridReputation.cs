using System;
using System.Xml.Serialization;
using WarBall.Events.Game;
using WarBall.Game;
using WarBall.Grid.Base;
using WarBall.Util;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridReputation : GridBase, IGridSetUIHandle, IGridCollisionHandle, IGridTurnSettlement
    {
        [XmlElement("UIName")]
        public string UIName { get; set; }

        [XmlElement("UpCount")]
        public int UpCount { get; set; }

        [XmlElement("TokenCount")]
        public int TokenCount { get; set; }

        [XmlElement("AttributesVal")]
        public int AttributesVal { get; set; }

        private int _count;

        public void OnCollisionHandle()
        {
            _count = GameBlackboard.Instance.SetCount(UIName, 1);
            if (_count > UpCount)
            {
                GameEvents.Instance.TriggerEvent<int, bool>(EGameEventType.UpdateToken, TokenCount);
                string attributeName = GetRandomAttributes.GetName();
                GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, attributeName
                    , EAttributeOperation.SetBase, GetRandomAttributes.GetValue(attributeName,AttributesVal));
                _count = GameBlackboard.Instance.SetCount(UIName, -_count);
            }
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

            GameBlackboard.Instance.CloseUI(UIName);
        }
    }
}
