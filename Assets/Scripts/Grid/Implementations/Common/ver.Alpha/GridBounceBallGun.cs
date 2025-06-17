using System;
using System.Xml.Serialization;
using UnityEngine;
using WarBall.Ball.Base;
using WarBall.Common;
using WarBall.Entity.Ball;
using WarBall.Events.Game;
using WarBall.Grid.Base;
using WarBall.XML;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridBounceBallGun : GridBase, IGridDestroyHandle , IGridReleasePrefabHandle
    {
        [XmlElement("BounceBallPrefab")]
        public string BounceBallPrefab { get; set; }

        public void OnReleasePrefab()
        {
            GameObject obj = PoolManager.Release(PrefabList.Instance.GetPrefab(BounceBallPrefab), Agent.transform.position);
            obj.GetComponent<BallEntityBase>().Set(AngleToVector2(Agent.transform.eulerAngles.z));
            GameEvents.Instance.TriggerEvent(EGameEventType.EntitySpawn, obj);
        }

        public void OnDestroyHandle() { }

        private Vector2 AngleToVector2(float angleDegrees)
        {
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            float x = Mathf.Cos(angleRadians);
            float y = Mathf.Sin(angleRadians);

            return new Vector2(x, y).normalized;
        }
    }
}
