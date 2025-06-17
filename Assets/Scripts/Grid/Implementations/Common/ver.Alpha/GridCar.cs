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
    public class GridCar : GridBase, IGridReleasePrefabHandle, IGridDoScaleCompleteHandle
    {
        [XmlElement("CarPrefab")]
        public string CarPrefab { get; set; }

        public void OnReleasePrefab()
        {
            GameObject obj = PoolManager.Release(PrefabList.Instance.GetPrefab(CarPrefab), Agent.transform.position, Quaternion.Euler(0, 0, Agent.transform.rotation.eulerAngles.z));
            GameEvents.Instance.TriggerEvent(EGameEventType.EntitySpawn, obj);
        }

        public void OnScaleComplete()
        {
            Agent.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-360f, 360f));
        }
    }
}
