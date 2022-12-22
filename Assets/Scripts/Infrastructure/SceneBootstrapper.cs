using Gameplay;
using Server;
using Services;
using UIControllers;
using UnityEngine;
using Zenject;

namespace GameModels.StateMachine
{
    public class SceneBootstrapper : MonoBehaviour
    {
        [Inject] private IGameStateController _gameStateController;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
        [Inject] private IJumperServerApi _jumperServerApi;
        [Inject] private INotificationService _notificationService;
        [Inject] private ILoadingCurtainsViewer _loadingCurtainsViewer;

        private void Start()
        {
            _loadingCurtainsViewer.Enable();
            _gameLoopStateMachine.Initialize();
            _gameStateController.Initialize();
            _jumperServerApi.Init(_notificationService);
        }
    }
}