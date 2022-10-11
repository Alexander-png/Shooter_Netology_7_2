using System;
using System.Collections;

namespace Lesson_7_5.NotificationSystem.Providers.Base
{
    public interface INotificationProvider
    {
        void RegisterNotification(string title, string body, DateTime fireTime, string channel);
        void ClearNotifications();
        IEnumerator RequestAutorization();
    }
}