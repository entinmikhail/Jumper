using UnityEngine;

namespace Popups
{
    public class PopupBase : MonoBehaviour
    {
        public void Open()
        {
            gameObject.SetActive(true);
            OnOpen();
        }

        protected virtual void OnOpen()
        {
        }
    }
}