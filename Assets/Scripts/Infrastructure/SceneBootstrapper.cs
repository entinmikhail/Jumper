using Gameplay;
using UnityEngine;
using Zenject;

namespace GameModels.StateMachine
{
    public class SceneBootstrapper : MonoBehaviour
    {
        [Inject] private IGameModel _gameModel;
        [Inject] private IGameStateController _gameStateController;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;

        private void Start()
        {
            _gameLoopStateMachine.Initialize();
            _gameStateController.Initialize(_gameModel.GameState);
        }
    }
}