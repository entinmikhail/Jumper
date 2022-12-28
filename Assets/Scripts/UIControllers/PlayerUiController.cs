using System.Globalization;
using Character;
using Configs;
using GameModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIControllers
{
    public interface IUiBus
    {
        void SetActiveFactor(bool value);
    }

    public class PlayerUiController : MonoBehaviour, IUiBus
    {
        [SerializeField] private FactorPanel _factorPanel;
        [SerializeField] private UIBetPanel _uiBetPanel;
        [Space]
        [SerializeField] private Button _jumpButton;
        [SerializeField] private BetterButton _cashOutButton;
        [SerializeField] private BetterButton _bonusBuyButton;
        [Space]
        [SerializeField] private TextMeshProUGUI _cashOutText;
        [SerializeField] private TextMeshProUGUI _bonusButtonText;

        [Inject] private IGameModel _gameModel;
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameConfigs _gameConfigs;
        [Inject] private IAccountModel _accountModel;
        [Inject] private IGameController _gameController;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private INotificationService _notificationService;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(OnJump);
            _cashOutButton.Button.onClick.AddListener(OnCashOut);
            _bonusBuyButton.Button.onClick.AddListener(OnBonusJump);
            _gameModel.GameStateChanged += OnGameStateChanged;
            _characterMover.MoveEnd += OnMoveEnd;
            _uiBetPanel.BetChanged += OnBetChanged;
        }

        private void OnBetChanged(float betAmount)
        {
            _bonusButtonText.text = (_gameConfigs.BonusFactor * betAmount).ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void OnMoveEnd()
        {
            RefreshCashText();
        }

        public void SetActiveFactor(bool value)
        {
            _factorPanel.gameObject.SetActive(value);
        }

        private void RefreshCashText()
        {
            _cashOutText.text =
                (_gameStorage.BetAmount * _gameStorage.CurrentFactor).ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void OnBonusJump()
        {
            if (_gameStorage.BetAmount <= 0)
            {
                _notificationService.ShowNotification("Сделайте ставку");
                return;
            }
            
            if (_gameStorage.BetAmount * _gameConfigs.BonusFactor > _accountModel.CurrentBalance)
            {
                _notificationService.ShowNotification("Пополните счёт");
                return;
            }

            if (_gameModel.GameState == GameState.PrepareGameState)
                _gameController.BonusJump();
        }

        private void OnGameStateChanged(GameState gameState)
        {
            _cashOutButton.gameObject.SetActive(true);
            _bonusBuyButton.gameObject.SetActive(false);

            if (gameState == GameState.Lose) 
                return;
            
            _bonusButtonText.text = (_gameConfigs.BonusFactor * _gameStorage.BetAmount).ToString("0.00", CultureInfo.InvariantCulture);
            RefreshCashText();
            _factorPanel.Refresh();
            _uiBetPanel.SetBet(_gameStorage.BetAmount);

            if (gameState != GameState.PrepareGameState)
                return;
            
            _cashOutButton.gameObject.SetActive(false);
            _bonusBuyButton.gameObject.SetActive(true);
            _factorPanel.SetFactor(_gameConfigs.DefaultFactor);
        }

        private void OnCashOut()
        {
            _gameController.Cashout();
        }

        private void OnJump()
        {
            if (_gameModel.GameState == GameState.Lose)
                return;

            if (_gameStorage.BetAmount > _accountModel.CurrentBalance)
            {
                _notificationService.ShowNotification("Пополните счёт");
                return;
            }
            
            if (_gameStorage.BetAmount <= 0)
            {
                _notificationService.ShowNotification("Сделайте ставку");
                return;
            }
            
            _gameStorage.BetAmount = _uiBetPanel.CurrentBet;

            if (_gameModel.GameState == GameState.PrepareGameState)
                _gameController.FirstJump();
            else
                _gameController.Jump();
        }
    }

    public enum ButtonType
    {
        Min,
        Max,
        Value
    }
}