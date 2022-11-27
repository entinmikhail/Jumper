using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Initialize();
    }

    public class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        
        [Inject] private IBootstrapState _bootstrapState;
        [Inject] private ILoadLevelState _loadLevelState;
        
        [Inject] private IGameplayState _gameplayState;
        [Inject] private IStartGameState _startGameState;
        [Inject] private IContinueGameState _continueGameState;
        
        public void Initialize()
        {
            _states = new Dictionary<Type, IExitableState>
            {
                { typeof(BootstrapState), _bootstrapState },
                { typeof(LoadLevelState), _loadLevelState  },
                
                
                { typeof(GameplayState), _gameplayState },
                { typeof(ContinueGameState), _continueGameState },
                { typeof(StartGameState), _startGameState },
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => _states[typeof(TState)] as TState;
    }
}