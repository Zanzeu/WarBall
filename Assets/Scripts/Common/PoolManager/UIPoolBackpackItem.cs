using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarBall.UI.Rest;

namespace WarBall.UI.Pool
{
    public class UIPoolBackpackItem : UIPool
    {
        public UIBackpackCell Cell { get; protected set; }

        protected override void OnInit()
        {
            base.OnInit();
            Cell = GetComponent<UIBackpackCell>();
        }
    }
}
