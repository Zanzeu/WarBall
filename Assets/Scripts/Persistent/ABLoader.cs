using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WarBall.Common;

namespace WarBall.Persistent
{
    public class ABLoader : PersistentSingleton<ABLoader>
    {
        private string ABPATH = Application.streamingAssetsPath + "/";
        private AssetBundle abMain = null;
        private AssetBundleManifest abManifest = null;
        private Dictionary<string,AssetBundle> bundles = new Dictionary<string,AssetBundle>();
        private void LoadAB(string abName)
        {
            if (abMain == null)
            {
                abMain = AssetBundle.LoadFromFile(ABPATH + "StandaloneWindows");
                abManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }

            AssetBundle ab = null;
            string[] strs = abManifest.GetAllDependencies(abName);
            for (int i = 0; i < strs.Length; i++)
            {
                if (!bundles.ContainsKey(strs[i]))
                {
                    ab = AssetBundle.LoadFromFile(ABPATH + strs[i]);
                    bundles[strs[i]] = ab;
                }
            }

            if (!bundles.ContainsKey(abName))
            {
                ab = AssetBundle.LoadFromFile(ABPATH + abName);
                bundles[abName] = ab;
            }
        }
        private IEnumerator LoadResourcesAsycCoroutine(string abName, string resName, UnityAction<Object> callBack, bool unLoad = false)
        {
            LoadAB(abName);

            AssetBundleRequest ab = bundles[abName].LoadAssetAsync(resName);

            yield return ab;
            
            callBack(ab.asset);   

            if (unLoad)
            {
                yield return null;
                yield return null;

                UnLoad(abName);
            }
        }
        private IEnumerator LoadResourcesAsycCoroutine(string abName, string resName,System.Type type ,UnityAction<Object> callBack, bool unLoad = false)
        {
            LoadAB(abName);

            AssetBundleRequest ab = bundles[abName].LoadAssetAsync(resName, type);

            yield return ab;

            callBack(ab.asset);

            if (unLoad)
            {
                yield return null;
                yield return null;

                UnLoad(abName);
            }
        }
        private IEnumerator LoadResourcesAsycCoroutine<T>(string abName, string resName, UnityAction<T> callBack, bool unLoad = false) where T : Object
        {
            LoadAB(abName);

            AssetBundleRequest ab = bundles[abName].LoadAssetAsync<T>(resName);

            yield return ab;

            callBack(ab.asset as T);

            if (unLoad)
            {
                yield return null;
                yield return null;

                UnLoad(abName);
            }
        }

        public Object LoadResources(string abName, string resName)
        {
            LoadAB(abName);

            Object obj = bundles[abName].LoadAsset(resName);

            return obj;
        }

        public Object LoadResources(string abName, string resName,System.Type type)
        {
            LoadAB(abName);

            Object obj = bundles[abName].LoadAsset(resName, type);

            return obj;
        }

        public T LoadResources<T>(string abName, string resName) where T : Object
        {
            LoadAB(abName);
            T obj = bundles[abName].LoadAsset<T>(resName);

            return obj;
        }

        public void LoadResourcesAsyc(string abName, string resName, UnityAction<Object> callBack, bool unLoad = false)
        {
            StartCoroutine(LoadResourcesAsycCoroutine(abName, resName, callBack, unLoad));
        }

        public void LoadResourcesAsyc(string abName, string resName, System.Type type, UnityAction<Object> callBack, bool unLoad = false)
        {
            StartCoroutine(LoadResourcesAsycCoroutine(abName, resName, type, callBack, unLoad));
        }

        public void LoadResourcesAsyc<T>(string abName, string resName, UnityAction<T> callBack, bool unLoad = false) where T : Object
        {
            StartCoroutine(LoadResourcesAsycCoroutine<T>(abName, resName, callBack, unLoad));
        }

        public void UnLoad(string abName)
        {
            if (bundles.ContainsKey(abName))
            {
                bundles[abName].Unload(false);
                bundles.Remove(abName);
            }
        }

        public void Clear()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            bundles.Clear();
            abMain = null;
            abManifest = null;
        }
    }
}