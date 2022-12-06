using System.Threading.Tasks;
using GameModels;
using Services;
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
        [Inject] private IUiLocker _uiLocker;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(OnJump);
            _cashOutButton.onClick.AddListener(OnCashOut);
            _gameModel.GameStateChanged += OnContinueGame;
        }

        private void OnContinueGame(GameState obj)
        {
            _uiBetPanel.CurrentBet = _gameModel.BetAmount;
        }

        private void OnCashOut()
        {
            _gameModel.CashOut();
        }

        private async void OnJump()
        {
            if (_uiBetPanel.CurrentBet <= 0)
            {
                Debug.LogError("Сделайте ставку");
                return;
            }
            
            _gameModel.BetAmount = _uiBetPanel.CurrentBet;
            _gameModel.Jump();
            
            _jumpButton.interactable = false;
            await Task.Delay(2000);
            _jumpButton.interactable = true;
        }
    }

    public enum ButtonType
    {
        Min,
        Max,
        Value
    }
}