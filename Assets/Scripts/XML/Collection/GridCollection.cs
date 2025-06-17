using System.Collections.Generic;
using System.Xml.Serialization;
using WarBall.Grid.Base;
using WarBall.Grid.Implementations;

namespace WarBall.XML
{
    [XmlRoot("Grid")]
    public class GridCollection
    {
        #region ==========Alpha==========

        [XmlElement("Dice", Type = typeof(GridDice))]
        [XmlElement("StoneWhell", Type = typeof(GridStoneWhell))]
        [XmlElement("Token",Type = typeof(GridToken))]
        [XmlElement("Armor", Type = typeof(GridArmor))]
        [XmlElement("Dumbbell", Type = typeof(GridDumbbell))]
        [XmlElement("Aim", Type = typeof(GridAim))]
        [XmlElement("BoxingGlove", Type = typeof(GridBoxingGlove))]
        [XmlElement("AddScoreMachine", Type = typeof(GridAddScoreMachine))]
        [XmlElement("WaterCandle", Type = typeof(GridWaterCandle))]
        [XmlElement("Glowdust", Type = typeof(GridGlowdust))]
        [XmlElement("MedicalKit",Type = typeof(GridMedicalKit))]
        [XmlElement("Bomb", Type = typeof(GridBomb))]
        [XmlElement("RandomScore",Type = typeof(GridRandomScore))]
        [XmlElement("Recast",Type = typeof(GridRecast))]
        [XmlElement("Lottery", Type = typeof(GridLottery))]
        [XmlElement("Retreat", Type = typeof(GridRetreat))]
        [XmlElement("Suppressor", Type = typeof(GridSuppressor))]
        [XmlElement("Car",Type = typeof(GridCar))]
        [XmlElement("Shotgun",Type = typeof(GridShotgun))]
        [XmlElement("RainCloud",Type = typeof(GridRainCloud))]
        [XmlElement("Brick",Type = typeof(GridBrick))]
        [XmlElement("Reputation", Type = typeof(GridReputation))]
        [XmlElement("BounceBallGun", Type = typeof(GridBounceBallGun))]

        #endregion

        public List<GridBase> Grids { get; set; } = new List<GridBase>();
    }
}
