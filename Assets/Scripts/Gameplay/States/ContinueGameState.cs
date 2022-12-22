﻿using Character;
using Configs;
using Gameplay;
using Platforms;
using Services;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IContinueGameState : IState
    {
    }

    public class ContinueGameState : IContinueGameState
    {
        [Inject] private IGameStorage _gameStorage;
        [Inject] private ICharacterMover _characterMover;
        [Inject] private IPlatformService _platformService;
        [Inject] private IGameLoopStateMachine _gameLoopStateMachine;
        [Inject] private ILoadingCurtainsViewer _loadingCurtainsViewer;

        public void Enter()
        {
            _loadingCurtainsViewer.Enable();
            for (int i = 1; i < _gameStorage.CurrentAltitude + 4; i++)
                _platformService.TryAddPlatformObjectByData(i);
            
            _characterMover.SetNumberPlatformWithoutAnimation(_gameStorage.CurrentAltitude);
            _characterMover.SetIdle();
            _characterMover.RotateCharacter();
            _gameLoopStateMachine.Enter<MainGameState>();
        }

        public void Exit()
        {
            _loadingCurtainsViewer.Disable();
        }
    }
}