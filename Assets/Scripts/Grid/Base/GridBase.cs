using WarBall.Config;
using WarBall.Agent;
using System.Xml.Serialization;
using System;

namespace WarBall.Grid.Base
{
    [Serializable]
    public class GridBase
    {
        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlAttribute("level")]
        public int Level { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Score")]
        public int Score { get; set; }

        [XmlElement("Joke")]
        public string Joke { get; set; }

        [XmlElement("IconPath")]
        public string IconPath { get; set; }

        [XmlElement("BcgPath")]
        public string BcgPath { get; set; }

        protected IAgent Agent;

        public virtual void OnInit(IAgent agent) { Agent = agent; }
    }
}
