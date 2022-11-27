using System;
using System.Collections.Generic;
using Infrastructure.States;
using UnityEngine;
using Zenject;

namespace GameModels.StateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void Initialize();
    }

    public class GameStateMachine : IGameStateMachine
    {
        private StateMachine _stateMachine;
        
        [Inject] private IBootstrapState _bootstrapState;
        [Inject] private ILoadLevelState _loadLevelState;
        [Inject] private IGameLoopState _gameLoopState;

        public void Initialize()
        {
            _stateMachine = new StateMachine(new Dictionary<Type, IExitableState>
            {
                { typeof(BootstrapState), _bootstrapState },
                { typeof(LoadLevelState), _loadLevelState },
                { typeof(GameLoopState), _gameLoopState },
            });
        }

        public void Enter<TState>() where TState : class, IState
        {
            _stateMachine.Enter<TState>();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            _stateMachine.Enter<TState, TPayload>(payload);
        }
    }
}