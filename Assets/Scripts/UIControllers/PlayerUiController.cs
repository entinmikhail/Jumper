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
        [SerializeField] private Button _cashOutButton;
        [SerializeField] private Button _bonusBuyButton;
        [Space]
        [SerializeField] private TextMeshProUGUI _cashOutText;
        [SerializeField] private TextMeshProUGUI _bonusButtonText;

        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameController _gameController;
        [Inject] private IGameModel _gameModel;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private INotificationService _notificationService;
        [Inject] private IGameConfigs _gameConfigs;

        private void Awake()
        {
            _jumpButton.onClick.AddListener(OnJump);
            _cashOutButton.onClick.AddListener(OnCashOut);
            _bonusBuyButton.onClick.AddListener(OnBonusJump);
            _gameModel.GameStateChanged += OnGameStateChanged;
            _characterMover.MoveEnd += OnMoveEnd;
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
            _gameController.ActivateBonusJump();
            _bonusBuyButton.interactable = false;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.PrepareGameState)
            {
                _cashOutButton.gameObject.SetActive(false);
                _bonusBuyButton.gameObject.SetActive(true);

            }
            else
            {
                _cashOutButton.gameObject.SetActive(true);
                _bonusBuyButton.gameObject.SetActive(false);
                _bonusBuyButton.interactable = true;
            }

            if (gameState == GameState.Lose)
                return;
            
            _bonusButtonText.text = _gameConfigs.BonusPrice.ToString("0.00", CultureInfo.InvariantCulture);
            RefreshCashText();
            _factorPanel.Refresh();
            _uiBetPanel.SetBet(_gameStorage.BetAmount);
        }

        private void OnCashOut()
        {
            _gameController.Cashout();
        }

        private void OnJump()
        {
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

            _jumpButton.interactable = false;

            _coroutineRunner.StartAfterDelay(2f, () =>
            {
                _jumpButton.interactable = true;
            });
        }
    }

    public enum ButtonType
    {
        Min,
        Max,
        Value
    }
}