using System.Collections.Generic;
using System.Xml.Serialization;
using WarBall.Ball.Base;
using WarBall.Ball.Implementations;
using WarBall.Ball.Implementations.Main;

namespace WarBall.XML
{
    [XmlRoot("Ball")]
    public class BallCollection
    {
        [XmlElement("MainWarrior", Type = typeof(BallMainWarrior))]
        public List<BallBase> Balls { get; set; } = new List<BallBase>();
    }
}
