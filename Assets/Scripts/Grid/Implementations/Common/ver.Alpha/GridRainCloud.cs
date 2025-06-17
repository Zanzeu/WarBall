using System;
using System.Xml.Serialization;
using UnityEngine;
using WarBall.Common;
using WarBall.Events.Game;
using WarBall.Grid.Base;
using WarBall.XML;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridRainCloud : GridBase, IGridReleasePrefabHandle
    {
        [XmlElement("RainCloudPrefab")]
        public string RainCloudPrefab { get; set; }

        public void OnReleasePrefab()
        {
            GameObject obj = PoolManager.Release(PrefabList.Instance.GetPrefab(RainCloudPrefab), Agent.transform.position);
            GameEvents.Instance.TriggerEvent(EGameEventType.EntitySpawn, obj);
        }
    }
}
