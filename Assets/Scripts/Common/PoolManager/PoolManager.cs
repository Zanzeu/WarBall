using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WarBall.Common
{
    public class PoolManager : MonoBehaviour
    {
        [LabelText("��")][SerializeField] private Pool[] ball;
        [LabelText("ש��")][SerializeField] private Pool[] grid;
        [LabelText("����")][SerializeField] private Pool[] enemy;
        [LabelText("VFX")][SerializeField] private Pool[] VFX;
        [LabelText("����")][SerializeField] private Pool[] tool;

        private static Dictionary<GameObject, Pool> dictionary;
        private static Transform prefabParent;

        private void Awake()
        {
            dictionary = new Dictionary<GameObject, Pool>();

            prefabParent = transform.Find("Pool:prefab");

            Init(ball,grid, enemy, VFX, tool);
        }

#if UNITY_EDITOR

        private void OnDestroy()
        {
            CheckPoolSize(ball,grid, enemy, VFX, tool);
        }

#endif

        private void CheckPoolSize(params Pool[][] pools)
        {
            foreach (var poolArray in pools)
            {
                foreach (var pool in poolArray)
                {
                    if (pool.RuntimeSize > pool.Size)
                    {
                        Debug.LogWarning(string.Format("{0}�ĳߴ�{1}���ڳ�ʼ����صĳߴ�{2}!",
                            pool.Prefab.name,
                            pool.RuntimeSize,
                            pool.Size));
                    }
                }
            }
        }

        private void Init(params Pool[][] pools)
        {
            foreach (var poolArray in pools)
            {
                foreach (var pool in poolArray)
                {
#if UNITY_EDITOR    
                    if (dictionary.ContainsKey(pool.Prefab))
                    {
                        Debug.LogError("������ͬԤ����:" + pool.Prefab.name);
                        continue;
                    }
#endif

                    dictionary.Add(pool.Prefab, pool);

                    Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;

                    poolParent.parent = transform;
                    pool.Init(poolParent);
                }
            }
        }

        public static void RegisterPool(GameObject prefab, int initialSize = 10)
        {
            if (!dictionary.ContainsKey(prefab))
            {
                Pool newPool = new Pool(prefab, initialSize);

                dictionary.Add(newPool.Prefab, newPool);

                newPool.Init(prefabParent);
            }
        }

        #region  =====�ͷ�Ԥ����=====

        public static GameObject Release(GameObject prefab)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("�޷��ҵ�Ԥ����:" + prefab.name);

                return null;
            }
#endif
            return dictionary[prefab].PreparedObject();
        }

        public static GameObject Release(GameObject prefab, Vector3 postion)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("�޷��ҵ�Ԥ����:" + prefab.name);

                return null;
            }
#endif
            return dictionary[prefab].PreparedObject(postion);
        }

        public static GameObject Release(GameObject prefab, Vector3 postion, Quaternion rotation)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("�޷��ҵ�Ԥ����:" + prefab.name);

                return null;
            }
#endif
            return dictionary[prefab].PreparedObject(postion, rotation);
        }

        public static GameObject Release(GameObject prefab, Vector3 postion, Vector3 localScale)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("�޷��ҵ�Ԥ����:" + prefab.name);

                return null;
            }
#endif
            return dictionary[prefab].PreparedObject(postion, localScale);
        }

        public static GameObject Release(GameObject prefab, Vector3 postion, Quaternion rotation, Vector3 localScale)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("�޷��ҵ�Ԥ����:" + prefab.name);

                return null;
            }
#endif
            return dictionary[prefab].PreparedObject(postion, rotation, localScale);
        }
        #endregion
    }
}