using System;
using System.Collections.Generic;
using UnityEngine;

namespace Popups
{
    public enum PopupType
    {
        WinPopup,
        LosePopup
    }

    public interface IPopupService
    {
        void ShowPopup(PopupType popupTypeType);
    }

    public class PopupService : MonoBehaviour, IPopupService
    {
        [SerializeField] private WinPopup _winPopup;
        [SerializeField] private LosePopup _losePopup;

        private Dictionary<PopupType, GameObject> _popupsByType = new();

        private void Awake()
        {
            _popupsByType = new Dictionary<PopupType, GameObject>()
            {
                { PopupType.LosePopup, _losePopup.gameObject },
                { PopupType.WinPopup, _winPopup.gameObject },
            };
        }

        public void ShowPopup(PopupType popupTypeType)
        {
            if (!_popupsByType.TryGetValue(popupTypeType, out var popup))
            {
                return;
            }
            
            popup.SetActive(true);
        }
    }
}