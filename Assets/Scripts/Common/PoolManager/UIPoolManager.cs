using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace WarBall.UI.Pool
{
    public enum UIPoolPrefab
    {
        BackpackItem,
        EffectItem,
        ValText,
    }

    public class UIPoolManager : MonoBehaviour
    {
        [Serializable]
        private struct PoolData
        {
            [LabelText("预制体")][SuffixLabel("Prefab")] public GameObject prefab;
            [LabelText("预制体生成数量")] public int size;
        }

        [LabelText("UI")][SerializeField] private List<PoolData> _pool = new List<PoolData>();
        private static Dictionary<UIPoolPrefab,GameObject> _Prefabs;

        public static Dictionary<UIPoolPrefab, Stack<UIPool>> PoolDic { get; private set; }

        private void Awake()
        {
            PoolDic = new Dictionary<UIPoolPrefab, Stack<UIPool>>();
            _Prefabs = new Dictionary<UIPoolPrefab, GameObject>();
        }

        public void Start()
        {
            Init(_pool);
        }

        private void Init(List<PoolData> pool_)
        {
            for (int i = 0; i < pool_.Count; i++)
            {
                UIPoolPrefab prefab = pool_[i].prefab.GetComponent<UIPool>().pool;
                if (!PoolDic.ContainsKey(prefab))
                {
                    GameObject parent = new GameObject();
                    Stack<UIPool> stack = new Stack<UIPool>();
                    parent.name = "Pool:" + pool_[i].prefab.name;
                    parent.transform.SetParent(transform);
                    for (int j = 0; j < pool_[i].size; j++)
                    {
                        GameObject obj = Instantiate(pool_[i].prefab);
                        obj.SetActive(false);
                        obj.transform.SetParent(parent.transform);
                        UIPool pool = obj.GetComponent<UIPool>();
                        pool.Init();
                        stack.Push(pool);
                    }

                    PoolDic[prefab] = stack;
                    _Prefabs[prefab] = pool_[i].prefab;
                }
                else
                {
                    Debug.LogWarning($"发现相同预制体{prefab}");
                }
            }
        }

        public static UIPool Release(UIPoolPrefab pool, Transform parent)
        {
            if (PoolDic.TryGetValue(pool, out Stack<UIPool> stack))
            {
                if (stack.Count == 0)
                {
                    GameObject newObj = Instantiate(_Prefabs[pool]);
                    UIPool newPool = newObj.GetComponent<UIPool>();
                    newPool.Init();
                    newPool.Outpool(parent);
                    Debug.LogWarning($"{pool}对象池扩容");

                    return newPool;            
                }
                else
                {
                    UIPool ui = stack.Pop();
                    ui.Outpool(parent);

                    return ui;
                }
            }

            Debug.LogError($"未发现预制体{pool}");

            return null;
        }
    }
}