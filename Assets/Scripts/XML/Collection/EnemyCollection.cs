using System.Collections.Generic;
using System.Xml.Serialization;
using WarBall.Enemy.Base;
using WarBall.Enemy.Implementations;

namespace WarBall.XML
{
    [XmlRoot("Enemy")]
    public class EnemyCollection
    {
        #region ==========Alpha==========

        [XmlElement("VileFluid", Type = typeof(EnemyVileFluid))]
        [XmlElement("DevilEye", Type = typeof(EnemyDevilEye))]
        [XmlElement("SmallGoblin", Type = typeof(EnemySmallGoblin))]
        [XmlElement("FloatGhost", Type = typeof(EnemyFloatGhost))]
        [XmlElement("Slime", Type = typeof(EnemySlime))]
        [XmlElement("SlimeSplit", Type = typeof(EnemySlimeSplit))]

        #endregion

        public List<EnemyBase> Enemys { get; set; } = new List<EnemyBase>();
    }
}
