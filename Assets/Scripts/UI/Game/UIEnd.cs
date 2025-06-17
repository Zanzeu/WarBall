using System.Collections;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using WarBall.Agent;
using WarBall.Game;
using WarBall.Persistent;
using WarBall.UI.Base;
using UnityEngine.SceneManagement;

namespace WarBall.UI.Game
{
    public class UIEnd : UIBase
    {
        private Canvas _canvas;
        [FoldoutGroup("组件")][LabelText("分数TMP")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _scoreTMP;

        private bool _canSwitch = false;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            GameManager.Instance.RegisterEvent(EGameStatus.End, (Action)OnEnd);    
        }

        private void OnDisable()
        {
            GameManager.Instance.UnregisterEvent(EGameStatus.End, (Action)OnEnd);
        }

        private void OnEnd()
        {
            DOTween.To(() => 0, x => _scoreTMP.text = x.ToString("D9"), GameProcess.Instance.Score, 1f)
                .SetEase(Ease.Linear).OnComplete(() =>
            {
                _canSwitch = true;
            });

            (GameBlackboard.Instance.GetData("ball") as GameObject).GetComponent<BallAgent>().Death();
            _canvas.enabled = true;
            StartCoroutine(LoadSceneCoroutine());
        }

        private IEnumerator LoadSceneCoroutine()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Single);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (Input.GetMouseButtonDown(0) && _canSwitch)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
