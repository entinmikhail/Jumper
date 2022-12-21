using Gameplay;
using Server;
using UIControllers;
using UnityEngine;
using Zenject;

namespace GameModels.StateMachine
{
    public class SceneBootstrapper : MonoBehaviour
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private IGameStateController _gameStateController;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
        [Inject] private IJumperServerApi _jumperServerApi;
        [Inject] private INotificationService _notificationService;

        private void Start()
        {
            _gameLoopStateMachine.Initialize();
            _gameStateController.Initialize(_gameModel.GameState);
            _jumperServerApi.Init(_notificationService);
        }
    }
}