namespace FSM
{
    public abstract class State<T>
    {
        protected StateMachine<T> _machine;
        protected T _context;

        public void SetMachineAndContext(StateMachine<T> machine, T context)
        {
            _machine = machine;
            _context = context;
            OnInitialized();
        }

        public virtual void OnInitialized() { }

        public virtual void Begin() { }

        public virtual void Reason() { }

        public virtual void Update(float deltaTime) { }

        public virtual void End() { }
    }
}