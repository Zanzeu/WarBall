using System;
using System.Xml.Serialization;
using WarBall.Agent;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridRandomScore : GridBase, IGridSwitchPositionHandle
    {
        [XmlElement("MinVal")]
        public int MinVal { get; set; }

        [XmlElement("MaxVal")]
        public int MaxVal { get; set; }

        public override void OnInit(IAgent agent)
        {
            base.OnInit(agent);
            Score = UnityEngine.Random.Range(MinVal, MaxVal);
        }

        public void OnSwitchPosition()
        {
            Score = UnityEngine.Random.Range(MinVal, MaxVal);
        }
    }
}
