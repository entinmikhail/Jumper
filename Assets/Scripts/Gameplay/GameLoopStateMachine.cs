using System;
using System.Collections.Generic;
using GameModels.StateMachine;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public interface IGameLoopStateMachine
    {
        void Initialize();
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
    }

    public class GameLoopStateMachine : IGameLoopStateMachine
    {
        private StateMachine _stateMachine;
        
        [Inject] private IPrepareGameState _prepareGameState;
        [Inject] private IMainGameState _mainGameState;
        [Inject] private IContinueGameState _continueGameState;
        [Inject] private IWinGameState _winGameState;
        [Inject] private ILoseGameState _loseGameState;
        [Inject] private IBonusGameState _bonusGameState;
        [Inject] private ILoadingState _loadingState;

        public void Initialize()
        {
            _stateMachine = new StateMachine(new Dictionary<Type, IExitableState>
            {
                { typeof(PrepareGameState), _prepareGameState },
                { typeof(ContinueGameState), _continueGameState },
                { typeof(MainGameState), _mainGameState },
                { typeof(WinGameState), _winGameState },
                { typeof(LoseGameState), _loseGameState },
                { typeof(BonusGameState), _bonusGameState },
                { typeof(LoadingState), _loadingState },
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