using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using Sirenix.OdinInspector;
using WarBall.UI.Base;
using WarBall.Persistent;
using WarBall.Game;
using WarBall.Events.Game;
using WarBall.Ball.Base;

namespace WarBall.UI.CanvasUI
{
    public class RestCanvas : UIBase
    {
        private Canvas _canvas;
        private Dictionary<string, Transform> _child;

        private Dictionary<string, TextMeshProUGUI> _attributesTMPs;
        private BallAttributes _attributes;
        private Transform _attributesParent;

        private TextMeshProUGUI _scoreTMP;
        private int _score;
        private int _wave;
        private int _prevScore;

        private Transform _levelParent;
        private TextMeshProUGUI _levelTMP;
        private TextMeshProUGUI _curXPTMP;
        private TextMeshProUGUI _nextXPTMP;
        private TextMeshProUGUI _waveTMP;
        private Image _curXPFill;

        [FoldoutGroup("经验设置")][LabelText("最大等级")][SerializeField] private int maxLevel;
        [FoldoutGroup("经验设置")] [LabelText("当前等级")] [SerializeField] private int level;
        [FoldoutGroup("经验设置")][LabelText("当前经验")][SerializeField] private int curXP;
        [FoldoutGroup("经验设置")][LabelText("目标经验")][SerializeField] private int nextXP;
        [DictionaryDrawerSettings(KeyLabel = "等级", ValueLabel = "所需经验")]
        [FoldoutGroup("经验设置")][LabelText("经验表")] public Dictionary<int, int> XPSetting;

        protected override void Awake()
        {
            base.Awake();
            _child = new Dictionary<string, Transform>();
            _attributesTMPs = new Dictionary<string, TextMeshProUGUI>();
        }

        private void Start()
        {
            OnInit();
        }

        private void OnInit()
        {
            _canvas = GetComponent<Canvas>();

            foreach (Transform c in transform)
            {
                _child[c.name] = c;
            }

            _child["store"].transform.Find("toBattle").GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.SwitchGameStatus(EGameStatus.Prepare);
            });

            _child["store"].transform.Find("update").GetComponent<Button>().onClick.AddListener(() =>
            {   
                if (!GameEvents.Instance.TriggerEvent<int,bool>(EGameEventType.UpdateToken, -1))
                {
                    return;
                }

                GameEvents.Instance.TriggerEvent(EGameEventType.UpdateStore);
            });

            _waveTMP = _child["store"].transform.Find("toBattle").GetChild(1).GetComponent<TextMeshProUGUI>();

            AttributesInit();
            ScoreInit();
            LevelInit();
        }

        private void OnEnable()
        {
            GameManager.Instance.RegisterEvent(EGameStatus.Prepare, (Action)CloseCanvas);
            GameManager.Instance.RegisterEvent(EGameStatus.Rest, (Action)OpenCanvas);
        }

        private void OnDisable()
        {
            GameManager.Instance.UnregisterEvent(EGameStatus.Prepare, (Action)CloseCanvas);
            GameManager.Instance.UnregisterEvent(EGameStatus.Rest, (Action)OpenCanvas);
        }

        private void OpenCanvas()
        {
            _wave = GameEvents.Instance.TriggerEvent<int>(EGameEventType.UpdateWave);
            _waveTMP.text = $"第{_wave + 1}回合";

            _canvas.enabled = true;

            StartCoroutine(OnOpenCanvasCoroutine());
        }

        private void CloseCanvas()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateHP, false);
            _canvas.enabled = false;
        }

        private void ScoreInit()
        {
            _scoreTMP = _child["score"].GetComponent<TextMeshProUGUI>();
            _score = 0;
            _prevScore = 0;
        }

        private void AttributesInit()
        {
            _attributesParent = _child["attributes"];

            foreach (Transform child in _attributesParent)
            {
                _attributesTMPs[child.name] = child.GetComponentInChildren<TextMeshProUGUI>();
            }
            _attributes = FindAnyObjectByType<BallAttributes>();
        }

        private void LevelInit()
        {
            _levelParent = _child["level"];

            _levelTMP = _levelParent.transform.Find("lv").GetComponent<TextMeshProUGUI>();
            _curXPTMP = _levelParent.transform.Find("curXP").GetComponent<TextMeshProUGUI>();
            _nextXPTMP = _levelParent.transform.Find("nextXP").GetComponent<TextMeshProUGUI>();

            nextXP = XPSetting[1];

            _levelTMP.text = $"Lv{level}";
            _curXPTMP.text = curXP.ToString();
            _nextXPTMP.text = $"/{nextXP}";

            _curXPFill = _levelParent.transform.Find("bcg").GetChild(0).GetComponent<Image>();
            _curXPFill.fillAmount = curXP / nextXP;
        }

        private void SetScore()
        {   
            DOTween.To(() => _score, x => _scoreTMP.text = x.ToString("D9"), GameProcess.Instance.Score, 1f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    _score = GameProcess.Instance.Score;
                });
        }

        private void SetLevel()
        {   
            if (level == maxLevel || GameProcess.Instance.Score - _prevScore <= 0)
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.UpdateStore);
                return;
            }

            int cur = curXP;
            curXP += ((GameProcess.Instance.Score - _prevScore) / 10);
            if (curXP >= nextXP)
            {
                curXP = nextXP;
                level = Mathf.Min(++level, maxLevel);
            }
            _prevScore = GameProcess.Instance.Score;
            int target = curXP;
            DOTween.To(() => cur, x => _curXPTMP.text = x.ToString(), target, 1f)
                .SetEase(Ease.Linear);

            _curXPFill.DOFillAmount((target * 1f) / nextXP, 1f).OnComplete(() =>
            {
                GameEvents.Instance.TriggerEvent(EGameEventType.UpdateStore);
                if (curXP >= nextXP)
                {   
                    if (level == maxLevel)
                    {
                        _levelTMP.text = $"Lv{level}";
                        _curXPTMP.text = "9999";
                        _nextXPTMP.text = "/Max";
                    }
                    else
                    {
                        _levelTMP.text = $"Lv{level}";
                        curXP = 0;
                        nextXP = XPSetting[level];
                        _curXPTMP.text = curXP.ToString();
                        _nextXPTMP.text = $"/{nextXP}";
                        _curXPFill.fillAmount = 0f;
                    }
                }
            });
        }

        private void SetAttributes()
        {
            //GameEvents.Instance.TriggerEvent<int, bool>(EGameEventType.UpdateCoin, 1);
            foreach (var tmp in _attributesTMPs)
            {
                SetText(tmp.Key, _attributes.Attributes[tmp.Key]);
            }
        }

        private void SetText(string name, AttributeForge forge)
        {
            string formattedApplyValue = "";

            if (!forge.Percentage)
            {
                formattedApplyValue = forge.ApplyValue.ToString();
            }
            else
            {
                formattedApplyValue = forge.ApplyValue.ToString("P2");
            }

            _attributesTMPs[name].text = $"{formattedApplyValue}";
        }

        private IEnumerator OnOpenCanvasCoroutine()
        {
            yield return null;
            SetScore();
            SetLevel();
            SetAttributes();
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateBackpack);
        }
    }
}
