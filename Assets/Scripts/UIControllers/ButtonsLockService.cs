using System;

namespace UIControllers
{
    public interface IButtonsLockService
    {
        event Action AllButtonsLocked;
        event Action AllButtonsUnlocked;
        event Action<bool> LostConnection;

        void LockAllButtons();
        void UnlockAllButtons();
        void SetLostConnectionButtonState(bool value);
    }

    public class ButtonsLockService : IButtonsLockService
    {
        public event Action AllButtonsLocked;
        public event Action AllButtonsUnlocked;
        public event Action<bool> LostConnection;

        public void LockAllButtons() => AllButtonsLocked?.Invoke();
        public void UnlockAllButtons() => AllButtonsUnlocked?.Invoke();

        public void SetLostConnectionButtonState(bool value)
        {
            LostConnection?.Invoke(value);
        }
    }
}