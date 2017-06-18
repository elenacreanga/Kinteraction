namespace Kinteraction
{
    public class BaseFacade<T>
    {
        protected readonly int DEFAULT_WINDOW_SIZE = 12;
        public int WindowSize { get; set; }

        public BaseFacade()
        {
            WindowSize = DEFAULT_WINDOW_SIZE;
        }

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
