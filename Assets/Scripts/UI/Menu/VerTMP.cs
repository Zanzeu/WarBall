using UnityEngine;
using TMPro;

namespace WarBall.UI.Menu
{
    public class VerTMP : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _TMP;
        private void Start()
        {
            _TMP.text =$"ver.{Application.version}";
        }
    }
}
