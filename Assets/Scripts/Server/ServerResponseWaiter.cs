using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Server
{
    public class ServerResponseWaiter : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onBusy;
        [SerializeField] private UnityEvent _onNotBusy;
        
        [Inject] private IJumperServerApi _jumperServerApi;

        private void OnEnable()
        {
            _jumperServerApi.RequestSentEvent += OnBusy;
            _jumperServerApi.ResponseReceivedEvent += OnNotBusy;
            
            if (_jumperServerApi.IsBusy)
                OnBusy();
            else
                OnNotBusy();
        }

        private void OnBusy()
        {
            _onBusy.Invoke();
        }

        private void OnNotBusy()
        {
            _onNotBusy.Invoke();
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