using System.Xml.Serialization;
using UnityEngine;
using WarBall.Agent;
using WarBall.Config;
using WarBall.Enemy.Base;
using WarBall.Events.Game;
using WarBall.Grid.Base;
using WarBall.Persistent;
using WarBall.UI.Base;
using WarBall.UI.Game;
using System;

namespace WarBall.Ball.Base
{
    [Serializable]
    public class BallBase
    {
        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("IconPath")]
        public string IconPath { get; set; }

        [XmlElement("HP")]
        public int HP { get; set; }

        [XmlElement("ATK")]
        public int ATK { get; set; }

        [XmlElement("DEF")]
        public int DEF { get; set; }

        [XmlElement("CRTR")]
        public float CRTR { get; set; }

        [XmlElement("DMG")]
        public float DMG { get; set; }

        [XmlElement("SCORE")]
        public float SCORE { get; set; }

        [XmlElement("COOL")]
        public float COOL { get; set; }

        [XmlElement("SPAWN")]
        public float SPAWN { get; set; }

        protected IAgent Agent;

        public virtual void OnInit(IAgent agent) { Agent = agent; }
    }
}
