using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace WarBall.Game
{
    public class ShowScoreValue : MonoBehaviour
    {
        private TextMeshPro _TMP;

        public void Set(float Y,int val)
        {   
            if (_TMP == null)
            {
                _TMP = GetComponent<TextMeshPro>();
            }

            _TMP.text = val.ToString();
            transform.DOMoveY(Y, 0.5f);
        }
    }
}
