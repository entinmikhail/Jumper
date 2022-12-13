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
        [Inject] private IStartGameState _startGameState;
        [Inject] private IContinueGameState _continueGameState;
        [Inject] private IWinGameState _winGameState;
        [Inject] private ILoseGameState _loseGameState;
        [Inject] private IBonusGameState _bonusGameState;

        public void Initialize()
        {
            _stateMachine = new StateMachine(new Dictionary<Type, IExitableState>
            {
                { typeof(PrepareGameState), _prepareGameState },
                { typeof(ContinueGameState), _continueGameState },
                { typeof(StartGameState), _startGameState },
                { typeof(WinGameState), _winGameState },
                { typeof(LoseGameState), _loseGameState },
                { typeof(BonusGameState), _bonusGameState },
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