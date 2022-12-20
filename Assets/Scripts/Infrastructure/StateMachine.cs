using System;
using System.Collections.Generic;

namespace GameModels.StateMachine
{
    public class StateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        public StateMachine(Dictionary<Type, IExitableState> states)
        {
            _states = states;
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