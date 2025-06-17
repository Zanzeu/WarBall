using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarBall.Util;
using WarBall.Grid.Base;
using WarBall.Persistent;
using WarBall.Config;

namespace WarBall.XML
{
    public class GridList
    {
        public static GridList Instance = new GridList();
        public Dictionary<string, GridBase> Data;
        public Dictionary<string, GridBase> GamingData;
        public Dictionary<string, GridSprite> Sprite;

        public string[] filter = { "grid_token" };

        public void Init()
        {
            Data = new Dictionary<string, GridBase>();
            GamingData = new Dictionary<string, GridBase>();
            Sprite = new Dictionary<string, GridSprite>();
            TextAsset xmlFile = Resources.Load<TextAsset>("XML/GridXML");
            if (xmlFile != null)
            {   
                var collection = XMLLoader.LoadFromXML<GridCollection>(xmlFile.text);
                foreach (var data in collection.Grids)
                {
                    Data[data.ID] = data;
                    GamingData[data.ID] = data;
                    Sprite[data.ID] = new GridSprite
                    {
                        Icon = ABLoader.Instance.LoadResources<Sprite>("grid", data.IconPath),
                        Bcg = ABLoader.Instance.LoadResources<Sprite>("grid", data.BcgPath)
                    };
                }

                Filter(); 
            }
            else
            {
                Debug.LogError("Î´ÕÒµ½GridXLM.xmlÎÄ¼þ");
            }
        }

        private void Filter()
        {   
            foreach (string id in filter)
            {
                GamingData.Remove(id);
            }   
        }

        public GridBase GetData(string id)
        {   
            if (Data.TryGetValue(id, out GridBase result))
            {
                return CloneFactory.CreateDeepCopy(result);
            }
            return null;
        }

        public T GetData<T>(string id) where T : GridBase
        {
            if (Data.TryGetValue(id, out GridBase result))
            {
                if (result is T concreteResult)
                {   
                    return CloneFactory.CreateDeepCopy(concreteResult);
                }
            }

            return null;
        }

        public GridBase CheckData(string id)
        {
            if (Data.TryGetValue(id, out GridBase result))
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

        public string RandomGetGamingId()
        {
            List<GridBase> list = GamingData.Values.ToList();

            return list[Random.Range(0, list.Count)].ID;
        }
    }
}
