using Character;
using GameModels;
using GameModels.StateMachine;
using Platforms;
using UIControllers;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public interface IBonusGameState : IState
    {
    }

    public class BonusGameState : IBonusGameState
    {
        [Inject] private IGameAnimatorController _gameAnimatorController;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IGameModel _gameModel;
        [Inject] private IPlatformService _platformService;

        public void Enter()
        {
            _gameAnimatorController.PlayBonusJump();
            
            for (int i = 1; i < _gameModel.CurrentAltitude + 3; i++)
                _platformService.TryAddPlatformObjectByData(i);
            
            _characterMover.SetNumberPlatform(_gameModel.CurrentAltitude);
            
            _coroutineRunner.StartAfterDelay(3, () =>
            {
                _gameLoopStateMachine.Enter<StartGameState>();
            });
            
        }

        public void Exit()
        {
        }
    }
}