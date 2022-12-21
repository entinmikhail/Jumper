using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UIControllers
{
    public interface INotificationService
    {
        void ShowNotification(string text);
    }

    public class NotificationService : MonoBehaviour, INotificationService
    {
        [SerializeField] private Notification _notificationPrefab;
        [SerializeField] private Transform _notificationRoot;
        [SerializeField] private float _notificationLifeTime;
        [SerializeField] private int _maxNotification;

        private Queue<Notification> _notificationStack  = new ();
        
        [Inject] private ICoroutineRunner _coroutineRunner;
        
        public void ShowNotification(string text)
        {
            var notification = Instantiate(_notificationPrefab, _notificationRoot);
            notification.SetMessage(text);
            notification.gameObject.SetActive(true);
            _notificationStack.Enqueue(notification);

            if (_notificationStack.Count > _maxNotification)
                RemoveNotification();
            else
                _coroutineRunner.StartAfterDelay(_notificationLifeTime, RemoveNotification);

        }

        private void RemoveNotification()
        {
            Destroy(_notificationStack.Dequeue().gameObject);
        }
    }
}