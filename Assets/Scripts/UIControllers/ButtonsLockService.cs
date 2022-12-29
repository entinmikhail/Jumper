using System;

namespace UIControllers
{
    public interface IButtonsLockService
    {
        event Action AllButtonsLocked;
        event Action AllButtonsUnlocked;
        void LockAllButtons();
        void UnlockAllButtons();
    }

    public class ButtonsLockService : IButtonsLockService
    {
        public event Action AllButtonsLocked;
        public event Action AllButtonsUnlocked;
        
        public void LockAllButtons()
        {
            AllButtonsLocked?.Invoke();
        }

        public void UnlockAllButtons()
        {
            AllButtonsUnlocked?.Invoke();
        }
    }
}