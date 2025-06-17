using System;
using System.Xml.Serialization;
using UnityEngine;
using WarBall.Agent;
using WarBall.Common;
using WarBall.Grid.Base;
using WarBall.Persistent;
using WarBall.XML;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridShotgun : GridBase, IGridCollisionHandle, IGridGunReplaceHandle
    {
        [XmlElement("FireBulletPrefab")]
        public string FireBulletPrefab { get; set; }

        [XmlElement("ReplaceIcon")]
        public string ReplaceIcon { get; set; }

        private bool _canFire;
        private float[] _angleOffsets = { -20f, -15f, -10f, -5f, 0f, 5f, 10f, 15f, 20f };
        [NonSerialized] private SpriteRenderer _spriteRenderer;
        [NonSerialized] private Sprite _normalIcon;
        [NonSerialized] private Sprite _replaceIcon;

        public override void OnInit(IAgent agent)
        {
            base.OnInit(agent);
            _canFire = true;
            _spriteRenderer = agent.GetComponent<GridAgent>()._spriteRenderer;
            _normalIcon = ABLoader.Instance.LoadResources<Sprite>("grid", IconPath);
            _replaceIcon = ABLoader.Instance.LoadResources<Sprite>("sprite", ReplaceIcon);
        }

        public void OnCollisionHandle()
        {
            if (_canFire)
            {
                _canFire = false;
                GameObject bullet = PrefabList.Instance.GetPrefab(FireBulletPrefab);
                float baseAngle = Agent.transform.rotation.eulerAngles.z;
                for (int i = 0; i < 9; i++)
                {
                    PoolManager.Release(bullet, Agent.transform.position, Quaternion.Euler(0, 0, baseAngle + _angleOffsets[i]));
                }

                _spriteRenderer.sprite = _replaceIcon;
            }
            else
            {
                OnReplace();
            }
        }

        public void OnReplace()
        {
            _canFire = true;
            _spriteRenderer.sprite = _normalIcon;
            Agent.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-360f, 360f));
        }
    }
}
