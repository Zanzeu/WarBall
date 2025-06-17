using System.Collections.Generic;
using UnityEngine;
using WarBall.Persistent;
using WarBall.Common;

namespace WarBall.XML
{
    public class PrefabList
    {
        public static PrefabList Instance = new PrefabList();
        public Dictionary<string, GameObject> Prefabs;

        public void Init()
        {
            Prefabs = new Dictionary<string, GameObject>();
        }

        public GameObject GetPrefab(string name)
        {
            if (Prefabs.TryGetValue(name, out GameObject obj))
            {
                return obj;
            }

            GameObject loadObj = ABLoader.Instance.LoadResources<GameObject>("prefab", name);
            if (loadObj != null)
            {
                Prefabs[name] = loadObj;
                PoolManager.RegisterPool(loadObj);
            }

            ABLoader.Instance.UnLoad("prefab");

            return loadObj;
        }

        public GameObject GetPrefab(string ab,string name)
        {
            if (Prefabs.TryGetValue(name, out GameObject obj))
            {
                return obj;
            }

            GameObject loadObj = ABLoader.Instance.LoadResources<GameObject>(ab, name);
            if (loadObj != null)
            {
                Prefabs[name] = loadObj;
                PoolManager.RegisterPool(loadObj);
            }

            ABLoader.Instance.UnLoad(ab);

            return loadObj;
        }
    }
}
