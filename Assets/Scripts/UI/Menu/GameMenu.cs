using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace WarBall.UI.Menu
{
    public class GameMenu : MonoBehaviour
    {
        [LabelText("点击开始TMP")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _clickTMP;
        [LabelText("点击开始TMP")][SuffixLabel("Image")][SerializeField] private Image _mask;
        private int _originalSceneIndex;

        private void Start()
        {
            _originalSceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadSceneCoroutine());
            _clickTMP.DOFade(0f, 0.8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        private IEnumerator LoadSceneCoroutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DOTween.Kill(_clickTMP);
                    _mask.enabled = true;
                    _mask.DOFade(1f, 1f).OnComplete(() =>
                    {
                        asyncLoad.allowSceneActivation = true;
                    });

                }

                yield return null;
            }
        }
    }
}
