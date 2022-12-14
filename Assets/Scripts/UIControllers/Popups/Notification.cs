using TMPro;
using UnityEngine;

namespace UIControllers
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        public void SetMessage(string text)
        {
            _textMeshProUGUI.text = text;
        }
    }
}