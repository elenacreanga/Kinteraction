namespace Kinteraction.Kinteract
{
    public class BaseFacade<T>
    {
        protected readonly int DefaultWindowSize = 12;

        public BaseFacade()
        {
            WindowSize = DefaultWindowSize;
        }

        public int WindowSize { get; set; }

        public bool IsRunning { get; private set; }

        public virtual void Start()
        {
            IsRunning = true;
        }

        public virtual void Stop()
        {
            IsRunning = false;
        }

        public virtual void Update(T value)
        {
            if (!IsRunning || value == null) return;
        }
    }
}