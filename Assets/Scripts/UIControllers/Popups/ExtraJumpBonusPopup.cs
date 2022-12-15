using Zenject;

namespace Popups
{
    public class ExtraJumpBonusPopup : PopupBase
    {
        [Inject] private ICoroutineRunner _coroutineRunner;

        protected override void OnOpen()
        {
            _coroutineRunner.StartAfterDelay(1, () =>
            {
                gameObject.SetActive(false);
            });
        } 
    }
}