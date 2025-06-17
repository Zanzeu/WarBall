using WarBall.Config;
using WarBall.Agent;
using System.Xml.Serialization;
using System;

namespace WarBall.Enemy.Base
{   
    public enum EnemySort
    {
        Normal,
        Elite,
        Boss,
    }

    [Serializable]
    public class EnemyBase
    {
        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("IconPath")]
        public string IconPath { get; set; }

        [XmlElement("BcgPath")]
        public string BcgPath { get; set; }

        [XmlElement("HP")]
        public int HP { get; set; }

        [XmlElement("ATK")]
        public int ATK { get; set; }

        [XmlElement("DEF")]
        public int DEF { get; set; }

        [XmlElement("SCORE")]
        public float SCORE { get; set; }

        [XmlElement("COOL")]
        public float COOL { get; set; }

        [XmlElement("LIFE")]
        public float LIFE { get; set; }

        [XmlElement("Sort")]
        public EnemySort Sort{ get; set; }

        protected IAgent Agent;
    }
}
