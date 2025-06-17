using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarBall.Common;

namespace WarBall.UI.Base
{
    public class UIManager : Singleton<UIManager>
    {
        private static Dictionary<string, UIBase> ui;
        private static Dictionary<string, Transform> child;

        protected override void Awake()
        {
            base.Awake();
            ui = new Dictionary<string, UIBase>();
            child = new Dictionary<string, Transform>();
        }

        private void Start()
        {
            foreach (Transform c in transform.GetChild(0))
            {
                child.Add(c.name, c);
            }
        }

        public static void RegisterUI(UIBase component)
        {
            ui[component.gameObject.name] = component;
        }

        public static void UnregisterUI(UIBase component)
        {
            if (ui.ContainsKey(component.gameObject.name))
            {
                ui.Remove(component.gameObject.name);
            }
        }

        public static T GetUI<T>(string name) where T : Component
        {
            if (ui.TryGetValue(name, out UIBase res))
            {
                return res.GetComponent<T>();
            }

            return null;
        }

        public static Transform GetUI(string name)
        {
            if (child.TryGetValue(name, out Transform t))
            {
                return t;
            }

            return null;
        }
    }
}
