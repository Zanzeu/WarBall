using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WarBall.UI.Pool
{   
    public enum UISort
    {
        Grid,
        Text,
    }

    public class UIPool : MonoBehaviour
    {
        public UIPoolPrefab pool;
        private Transform _parent;
       public RectTransform RectTransform { get; set; }
        public UISort sort;
        public Image Bcg { get; set; }
        public Image Icon{ get; set; }
        public TextMeshProUGUI TMP { get; set; }

        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {   
            _parent = transform.parent;
            RectTransform = GetComponent<RectTransform>();

            if (sort == UISort.Grid)
            {
                Bcg = GetComponent<Image>();
                Icon = transform.Find("icon").GetComponent<Image>();
            }
            else if (sort == UISort.Text)
            {
                TMP = GetComponent<TextMeshProUGUI>();
            }
        }

        public void Enterpool()
        {
            if (!UIPoolManager.PoolDic[pool].Contains(this))
            {
                gameObject.SetActive(false);
                UIPoolManager.PoolDic[pool].Push(this);
                transform.SetParent(_parent);
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}对象重复入池");
            }
        }

        public void Outpool(Transform parent)
        {
            transform.SetParent(parent);
            RectTransform.anchoredPosition = Vector3.zero;
            RectTransform.localScale = Vector3.one;
            gameObject.SetActive(true);
        }
    }
}