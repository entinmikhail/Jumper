using System;
using Character;
using GameModels;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _cashOutText;

        [Inject] private IGameStorage _gameStorage;
        [Inject] private IGameController _gameController;
        [Inject] private IGameModel _gameModel;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private INotificationService _notificationService;
        
        private void Awake()
        {
            _jumpButton.onClick.AddListener(OnJump);
            _cashOutButton.onClick.AddListener(OnCashOut);
            _bonusBuyButton.onClick.AddListener(OnBonusJump);
            
            _gameModel.GameStateChanged += OnContinueGame;
            _characterMover.MoveEnd += OnMoveEnd;
        }

        private void OnMoveEnd()
        {
            _cashOutText.text = $"$ {Math.Round(_gameStorage.BetAmount * _gameStorage.CurrentCoefficient, 2)}";
        }

        private void OnBonusJump()
        {
            _gameController.ActivateBonusJump();
            _bonusBuyButton.interactable = false;
        }

        private void OnContinueGame(GameState gameState)
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
            
            _uiBetPanel.CurrentBet = _gameStorage.BetAmount;
        }

        private void OnCashOut()
        {
            _gameController.Cashout();
        }

        private void OnJump()
        {
            if (_uiBetPanel.CurrentBet <= 0)
            {
                _notificationService.ShowNotification("Сделайте ставку");
                return;
            }
            
            _gameStorage.BetAmount = _uiBetPanel.CurrentBet;

            if (_gameModel.GameState == GameState.PrepareGameState)
                _gameController.FirstJump(_uiBetPanel.CurrentBet);
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