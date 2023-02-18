using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lost.Utils.StateMachine
{
    public abstract class StateMachine<T> : IDisposable
        where T : class
    {
        [CanBeNull] private T _currentState;

        private readonly Dictionary<Type, T> _states = new();

        public void AddState<TState>(TState state)
            where TState : T, IState
        {
            _states.Add(state.GetType(), state);
        }

        public void AddStateWithPayload<TState, TPayload>(TState state)
            where TState : T, IStateWithPayload<TPayload>
            where TPayload : class
        {
            _states.Add(state.GetType(), state);
        }

        public void EnterToState<TType>() where TType : IState
        {
            if (_currentState is IStateWithExit stateWithExit)
                stateWithExit.Exit();

            Type stateType = typeof(TType);
            if (!_states.TryGetValue(stateType, out T state))
                throw new Exception($"There is not state with type '{stateType}'");

            _currentState = state;

            if (state is IState stateWithoutPayload)
                stateWithoutPayload.Enter();

            throw new Exception($"There is type '{stateType}' without '{nameof(IState)}' interface");
        }

        public void EnterToState<TType, TPayload>(TPayload payload)
            where TType : IStateWithPayload<TPayload>
            where TPayload : class
        {
            if (_currentState is IStateWithExit stateWithExit)
                stateWithExit.Exit();

            Type stateType = typeof(TType);
            if (!_states.TryGetValue(stateType, out T state))
                throw new Exception($"There is not state with type '{stateType}'");

            _currentState = state;
            if (state is IStateWithPayload<TPayload> stateWithoutPayload)
                stateWithoutPayload.Enter(payload);

            throw new Exception($"There is type '{stateType}' without '{nameof(IStateWithPayload<TPayload>)}' interface");
        }

        public void Dispose()
        {
            if (_currentState is IDisposable disposable)
                disposable.Dispose();

            _currentState = null;
        }
    }
}