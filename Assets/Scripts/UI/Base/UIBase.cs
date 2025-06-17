using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace WarBall.UI.Base
{
    public class UIBase : SerializedMonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.RegisterUI(this);
        }

        private void OnDestroy()
        {
            UIManager.UnregisterUI(this);
        }
    }
}
