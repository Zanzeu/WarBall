using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarBall.Events.Game;
using WarBall.Game;
using WarBall.Persistent;
using WarBall.UI.Game;
using WarBall.XML;
using WarBall.Common;

namespace WarBall.Ball.Base
{
    [RequireComponent(typeof(UIAttributes))]
    public class BallAttributes : Singleton<BallAttributes>
    {   
        public Dictionary<string, AttributeForge> Attributes { get; private set; }

        public Dictionary<string, AttributeForge> GetAllAttribute() => Attributes;

        public void Init() => OnInit();

        protected override void Awake()
        {
            base.Awake();
            Attributes = new Dictionary<string, AttributeForge>();
        }

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            GameManager.Instance.RegisterEvent(EGameStatus.Rest, (Action)OnRest);
        }

        private void OnDisable()
        {
            GameManager.Instance.RegisterEvent(EGameStatus.Rest, (Action)OnRest);
        }

        private void OnInit()
        {
            Attributes.Add("HP", new HPForge("HP",9999f,BallList.Instance.CheckData(GameManager.Instance.BallID).HP));
            Attributes.Add("ATK", new AttributeForge("ATK", BallList.Instance.CheckData(GameManager.Instance.BallID).ATK, 9999f, false));
            Attributes.Add("DEF", new AttributeForge("DEF", BallList.Instance.CheckData(GameManager.Instance.BallID).DEF, 9999f, false));
            Attributes.Add("CRTR", new AttributeForge("CRTR", BallList.Instance.CheckData(GameManager.Instance.BallID).CRTR, 1f, true));
            Attributes.Add("DMG", new AttributeForge("DMG", BallList.Instance.CheckData(GameManager.Instance.BallID).DMG, 9999f, true));
            Attributes.Add("SCORE", new AttributeForge("SCORE", BallList.Instance.CheckData(GameManager.Instance.BallID).SCORE, 9999f, true));
            Attributes.Add("COOL", new AttributeForge("COOL", BallList.Instance.CheckData(GameManager.Instance.BallID).COOL, 9999f, true));
            Attributes.Add("SPAWN", new AttributeForge("SPAWN", BallList.Instance.CheckData(GameManager.Instance.BallID).SPAWN, 9999f, true));
        }

        public AttributeForge SetAttributes(string attribute,EAttributeOperation target,float val)
        {
            if (target == EAttributeOperation.SetBase)
            {
                Attributes[attribute].SetBaseValue(val);
            }
            else if (target == EAttributeOperation.SetTemp)
            {
                Attributes[attribute].SetTempValue(val);
            }
            else if (target == EAttributeOperation.SetMax)
            {
                Attributes[attribute].SetMaxValue(val, out float curVal);
            }
            else if (target == EAttributeOperation.SetCur)
            {
               Attributes[attribute].SetCurValue(val);
            }

            return Attributes[attribute];
        }

        private void OnRest()
        {
            foreach (var attr in Attributes)
            {
                attr.Value.ResetTempValue();
                GameEvents.Instance.TriggerEvent(EGameEventType.ResetAttributes, attr.Key);
            }

            Attributes["HP"].ResetCurValue();
        }
    }
}
