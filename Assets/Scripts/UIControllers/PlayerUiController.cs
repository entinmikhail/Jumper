using GameModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIControllers
{
    public class PlayerUiController : MonoBehaviour
    {
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _cashOutButton;
        [SerializeField] private Button _bonusBuyButton;
        [SerializeField] private UIBetPanel _uiBetPanel;

        [Inject] private IGameModel _gameModel;

        
        private void Awake()
        {
            _jumpButton.onClick.AddListener(OnJump);
            _cashOutButton.onClick.AddListener(OnCashOut);
        }

        private void OnCashOut()
        {
            _gameModel.CashOut();
        }

        private void OnJump()
        {
            if (_uiBetPanel.CurrentBet <= 0)
            {
                Debug.LogError("Сделайте ставку");
                return;
            }
            _gameModel.BetAmount = _uiBetPanel.CurrentBet;
            _gameModel.Jump();
        }
    }

    public enum ButtonType
    {
        Min,
        Max,
        Value
    }
}