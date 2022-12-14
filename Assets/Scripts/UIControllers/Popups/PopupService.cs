using System;
using System.Collections.Generic;
using UnityEngine;

namespace Popups
{
    public enum PopupType
    {
        WinPopup,
        LosePopup,
        ExtraFactorBonusPopup,
        ExtraJumpBonusPopup,
    }

    public interface IPopupService
    {
        void ShowPopup(PopupType popupTypeType);
    }

    public class PopupService : MonoBehaviour, IPopupService
    {
        [SerializeField] private WinPopup _winPopupBase;
        [SerializeField] private LosePopup _losePopup;
        [SerializeField] private ExtraFactorBonusPopup _extraFactorBonusPopup;
        [SerializeField] private ExtraJumpBonusPopup _extraJumpBonusPopup;

        private Dictionary<PopupType, PopupBase> _popupsByType = new();

        private void Awake()
        {
            _popupsByType = new Dictionary<PopupType, PopupBase>()
            {
                { PopupType.LosePopup, _losePopup },
                { PopupType.WinPopup, _winPopupBase },
                { PopupType.ExtraFactorBonusPopup, _extraFactorBonusPopup },
                { PopupType.ExtraJumpBonusPopup, _extraJumpBonusPopup },
            };
            
        }

        public void ShowPopup(PopupType popupTypeType)
        {
            if (!_popupsByType.TryGetValue(popupTypeType, out var popup))
            {
                return;
            }
            
            popup.Open();
        }
    }
}