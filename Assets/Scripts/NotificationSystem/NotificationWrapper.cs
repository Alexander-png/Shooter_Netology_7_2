using Lesson_7_5.NotificationSystem.Providers;
using Lesson_7_5.NotificationSystem.Providers.Base;
using System;
using UnityEngine;

namespace Lesson_7_5.NotificationSystem
{
    public class NotificationWrapper : MonoBehaviour
    {
        [SerializeField]
        private string[] _channels = { "default" };

        private INotificationProvider _notificationProvider;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                _notificationProvider = new AndroidNotificationProvider(_channels);
            }
            else
            {
                _notificationProvider = new IOSNotificationProvider();
            }
            _notificationProvider.ClearNotifications();
            DontDestroyOnLoad(this.gameObject);
            _notificationProvider.RequestAutorization();
        }

        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                RegisterNotifications();
            }
            else
            {
                _notificationProvider?.ClearNotifications();
            }
        }

        private void RegisterNotifications()
        {
            _notificationProvider.RegisterNotification("title", "body", DateTime.Now.AddHours(1), _channels[0]);
        }
    }
}