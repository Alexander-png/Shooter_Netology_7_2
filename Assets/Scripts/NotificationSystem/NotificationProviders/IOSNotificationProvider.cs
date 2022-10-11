using Lesson_7_5.NotificationSystem.Providers.Base;
using System;
using System.Collections;
using Unity.Notifications.iOS;
using UnityEngine;

namespace Lesson_7_5.NotificationSystem.Providers
{
    public class IOSNotificationProvider : INotificationProvider
    {
        public void ClearNotifications()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();
        }

        public void RegisterNotification(string title, string body, DateTime fireTime, string channel)
        {
            iOSNotificationCenter.ScheduleNotification(new iOSNotification()
            {
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
                CategoryIdentifier = channel,
                Trigger = new iOSNotificationCalendarTrigger()
                {
                    Year = fireTime.Year,
                    Month = fireTime.Month,
                    Day = fireTime.Day,
                    Minute = fireTime.Minute,
                    Second = fireTime.Second,
                    Repeats = true,

                }
            });
        }

        public IEnumerator RequestAutorization()
        {
            AuthorizationOption option = AuthorizationOption.Alert | AuthorizationOption.Sound;

            using (AuthorizationRequest request = new AuthorizationRequest(option, false))
            {
                while (!request.IsFinished)
                {
                    yield return null;
                }

#if UNITY_EDITOR
                Debug.Log($"Result : {request.IsFinished}");
#endif
            }
        }
    }
}