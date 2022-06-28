using System;
using System.Collections.Generic;

namespace FSM
{
    public class StateMachine<T>
    {
        public event Action OnStateChanged;

        public State<T> CurrentState => _currentState;
        public State<T> PreviousState;
        public float ElapsedTimeInState = 0f;

        protected State<T> _currentState;
        protected T _context;

        private Dictionary<Type, State<T>> _states = new Dictionary<Type, State<T>>();

        public StateMachine(T context, State<T> initialState)
        {
            _context = context;

            AddState(initialState);
            _currentState = initialState;
        }

        public virtual void Update(float deltaTime)
        {
            ElapsedTimeInState += deltaTime;
            _currentState.Reason();
            _currentState.Update(deltaTime);
        }

        public void AddState(State<T> state)
        {
            _states[state.GetType()] = state;
        }

        public virtual R GetState<R>() where R : State<T>
        {
            var type = typeof(R);
            if (!_states.ContainsKey(type))
            {
                Console.WriteLine($"State machine doesn't have any state of name {type}");
                return null;
            }

            return (R)_states[type];
        }

        public R ChangeState<R>() where R : State<T>
        {
            var newType = typeof(R);
            if (_currentState.GetType() == newType)
            {
                return _currentState as R;
            }

            if (_currentState != null)
            {
                _currentState.End();
            }

            if (!_states.ContainsKey(newType))
            {
                Console.WriteLine($"State machine doesn't have any state of name {newType}");
                return null;
            }

            ElapsedTimeInState = 0f;
            PreviousState = _currentState;
            _currentState = _states[newType];
            _currentState.Begin();

            OnStateChanged?.Invoke();

            return _currentState as R;
        }
    }
}