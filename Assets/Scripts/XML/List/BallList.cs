using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarBall.Util;
using WarBall.Ball.Base;
using WarBall.Persistent;

namespace WarBall.XML
{
    public class BallList
    {   
        public static BallList Instance = new BallList();
        public Dictionary<string, BallBase> Data;
        public Dictionary<string, Sprite> Icon;

        public void Init()
        {
            Data = new Dictionary<string, BallBase>();
            Icon = new Dictionary<string, Sprite>();
            TextAsset xmlFile = Resources.Load<TextAsset>("XML/BallXML");
            if (xmlFile != null)
            {
                var collection = XMLLoader.LoadFromXML<BallCollection>(xmlFile.text);
                foreach (var data in collection.Balls)
                {
                    Data[data.ID] = data;
                    Icon[data.ID] = ABLoader.Instance.LoadResources<Sprite>("ball", data.IconPath);
                }
            }
            else
            {
                Debug.LogError("Î´ÕÒµ½GridXLM.xmlÎÄ¼þ");
            }
        }

        public BallBase GetData(string id)
        {
            if (Data.TryGetValue(id, out BallBase result))
            {
                return CloneFactory.CreateDeepCopy(result);
            }
            return null;
        }

        public T GetData<T>(string id) where T : BallBase
        {
            if (Data.TryGetValue(id, out BallBase result))
            {
                if (result is T concreteResult)
                {
                    return CloneFactory.CreateDeepCopy(concreteResult);
                }
            }

            return null;
        }

        public BallBase CheckData(string id)
        {
            if (Data.TryGetValue(id, out BallBase result))
            {
                return result;
            }

            return null;
        }

        public Sprite GetIcon(string id)
        {
            if (Icon.TryGetValue(id, out Sprite result))
            {
                return result;
            }

            return null;
        }

        public BallBase RandomGetData()
        {
            List<BallBase> list = Data.Values.ToList();

            return list[Random.Range(0, list.Count)];
        }
    }
}
