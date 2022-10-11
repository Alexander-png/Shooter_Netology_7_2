using Lesson_7_5.NotificationSystem.Providers.Base;
using System;
using System.Collections;
using Unity.Notifications.Android;

namespace Lesson_7_5.NotificationSystem.Providers
{
    public class AndroidNotificationProvider : INotificationProvider
    {
        public AndroidNotificationProvider(string[] channels)
        {
            foreach (var channel in channels)
            {
                AndroidNotificationCenter.RegisterNotificationChannel(new AndroidNotificationChannel()
                {
                    Id = channel,
                    Name = channel,
                    Importance = Importance.Default,
                });
            }
        }

        public void ClearNotifications()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }

        public void RegisterNotification(string title, string body, DateTime fireTime, string channel)
        {
            AndroidNotificationCenter.SendNotification(new AndroidNotification()
            {
                Title = title,
                Text = body,
                FireTime = fireTime,
            }, channel);
        }

        public IEnumerator RequestAutorization()
        {
            return null;
        }
    }
}