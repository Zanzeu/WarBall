using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarBall.Util;
using WarBall.Enemy.Base;
using WarBall.Persistent;
using WarBall.Config;

namespace WarBall.XML
{
    public class EnemyList
    {
        public static EnemyList Instance = new EnemyList();
        public Dictionary<string, EnemyBase> Data;
        public Dictionary<string, GridSprite> Sprite;

        public void Init()
        {
            Data = new Dictionary<string, EnemyBase>();
            Sprite = new Dictionary<string, GridSprite>();
            TextAsset xmlFile = Resources.Load<TextAsset>("XML/EnemyXML");
            if (xmlFile != null)
            {
                var collection = XMLLoader.LoadFromXML<EnemyCollection>(xmlFile.text);
                foreach (var data in collection.Enemys)
                {   
                    Data[data.ID] = data;
                    Sprite[data.ID] = new GridSprite
                    {
                        Icon = ABLoader.Instance.LoadResources<Sprite>("enemy", data.IconPath),
                        Bcg = ABLoader.Instance.LoadResources<Sprite>("grid", data.BcgPath)
                    };
                }
            }
            else
            {
                Debug.LogError("Î´ÕÒµ½GridXLM.xmlÎÄ¼þ");
            }
        }

        public EnemyBase GetData(string id)
        {
            if (Data.TryGetValue(id, out EnemyBase result))
            {
                return CloneFactory.CreateDeepCopy(result);
            }
            return null;
        }

        public T GetData<T>(string id) where T : EnemyBase
        {
            if (Data.TryGetValue(id, out EnemyBase result))
            {
                if (result is T concreteResult)
                {
                    return CloneFactory.CreateDeepCopy(concreteResult);
                }
            }

            return null;
        }

        public EnemyBase CheckData(string id)
        {
            if (Data.TryGetValue(id, out EnemyBase result))
            {
                return result;
            }

            return null;
        }

        public GridSprite GetSprite(string id)
        {
            if (Sprite.TryGetValue(id, out GridSprite result))
            {
                return result;
            }

            return new GridSprite();
        }

        public EnemyBase RandomGetData()
        {
            List<EnemyBase> list = Data.Values.ToList();

            return list[Random.Range(0, list.Count)];
        }
    }
}
