using UIControllers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Server
{
    public class ServerResponseWaiter : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onBusy;
        [SerializeField] private UnityEvent _onNotBusy;

        private bool _lockByService;
        [Inject] private IJumperServerApi _jumperServerApi;
        [Inject] private IButtonsLockService _buttonsLockService;

        private void OnEnable()
        {
            _jumperServerApi.RequestSentEvent += OnBusy;
            _jumperServerApi.ResponseReceivedEvent += OnNotBusy;
            
            _buttonsLockService.AllButtonsLocked += OnAllButtonsLocked;
            _buttonsLockService.AllButtonsUnlocked += OnAllButtonsUnlocked;
            
            if (_jumperServerApi.IsBusy)
                OnBusy();
            else
                OnNotBusy();
        }

        private void OnAllButtonsUnlocked()
        {
            _lockByService = false;
            _onNotBusy?.Invoke();
        }

        private void OnAllButtonsLocked()
        {
            _lockByService = true;
            _onBusy?.Invoke();
        }

        private void OnBusy()
        {
            _onBusy?.Invoke();
        }

        private void OnNotBusy()
        {
            if (_lockByService)
                return;
            
            _onNotBusy?.Invoke();
        }

        private void OnDisable()
        {
            _jumperServerApi.RequestSentEvent -= OnBusy;
            _jumperServerApi.ResponseReceivedEvent -= OnNotBusy;
        }

        private void OnDestroy()
        {
            if (!gameObject.activeSelf)
                return;
            
            _jumperServerApi.RequestSentEvent -= OnBusy;
            _jumperServerApi.ResponseReceivedEvent -= OnNotBusy;
        }
    }
}