namespace Services
{
    public interface IUiLocker
    {
        // void Lock();
        // void Unlock();
        // bool IsUnlock();
    }
    
    public class UiLocker : IUiLocker
    {
        private bool _isLock;
        
        public void Lock()
        {
            _isLock = true;
        }

        public void Unlock()
        {
            _isLock = false;
        }

        public bool IsUnlock()
        {
            return !_isLock;
        }
    }
}