using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarBall.UI.Base;
using WarBall.Persistent;
using System;

namespace WarBall.UI.CanvasUI
{
    public class GameCanvas : UIBase
    {
        private Canvas _canvas;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            GameManager.Instance.RegisterEvent(EGameStatus.Prepare,(Action)OpenCanvas);
            GameManager.Instance.RegisterEvent(EGameStatus.Rest, (Action)CloseCanvas);
        }

        private void OnDisable()
        {
            GameManager.Instance.UnregisterEvent(EGameStatus.Prepare, (Action)OpenCanvas);
            GameManager.Instance.UnregisterEvent(EGameStatus.Rest, (Action)CloseCanvas);
        }

        private void OpenCanvas()
        {
            _canvas.enabled = true;
        }

        private void CloseCanvas()
        {
            _canvas.enabled = false;
        }
    }
}
