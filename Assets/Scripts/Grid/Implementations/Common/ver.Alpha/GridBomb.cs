using System;
using System.Xml.Serialization;
using UnityEngine;
using WarBall.Agent;
using WarBall.Common;
using WarBall.Events.Game;
using WarBall.Grid.Base;
using WarBall.XML;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridBomb : GridBase,IGridDestroyDelayHandle, IGridCollisionHandle
    {
        [XmlElement("MaxCollisionCount")]
        public int MaxCollisionCount { get; set; }

        [XmlElement("ExplodeBullet")]
        public string ExplodeBullet { get; set; }

        private int _curCollisionCount;

        public override void OnInit(IAgent agent)
        {
            base.OnInit(agent);
            _curCollisionCount = 0;
        }

        public void OnCollisionHandle()
        {
            _curCollisionCount++;
        }

        public bool OnDestroyDelayHandle() 
        {
            if (_curCollisionCount >= MaxCollisionCount)
            {
                GameObject prefab = PrefabList.Instance.GetPrefab(ExplodeBullet);

                for(int i = 0; i < 360; i+= 45)
                {
                    GameObject obj = PoolManager.Release(prefab, Agent.transform.position, Quaternion.Euler(0, 0, i));
                    GameEvents.Instance.TriggerEvent(EGameEventType.EntitySpawn, obj);
                }
                return true;
            }

            return false;
        }
    }
}