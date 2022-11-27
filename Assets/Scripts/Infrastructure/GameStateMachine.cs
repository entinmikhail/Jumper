using System;
using System.Collections.Generic;

namespace GameModels.StateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : IState;
    }

    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameStateMachine()
        {
            _states = new Dictionary<Type, IState>
            {
                { typeof(BootstrapState), new BootstrapState() },
                { typeof(GameplayState), new GameplayState() },
                { typeof(ContinueGameState), new ContinueGameState() },
                { typeof(LoadLevelState), new LoadLevelState() },
                { typeof(StartGameState), new StartGameState() },
            };
        }

        public void Enter<TState>() where TState : IState
        {
            _activeState?.Exit();
            _activeState = _states[typeof(TState)];
            _activeState.Enter();
        }
    }
}