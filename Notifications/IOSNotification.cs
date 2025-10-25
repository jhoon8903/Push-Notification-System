
using Projects.Scripts.Localize;
#if UNITY_IOS
using System;
using Cysharp.Threading.Tasks;
using Unity.Notifications.iOS;
#endif

namespace Projects.Scripts.Plugin.Notifications
{
#if UNITY_IOS
    public class IOSNotification : INotification
    {

        private bool _notificationsScheduled;

        public async void RequestAuthorization()
        {
            using AuthorizationRequest request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);
            while (!request.IsFinished) await UniTask.Yield();
        }

        public void RegisterNotification() { }

        #region INTERVAL

        private void SendTimeIntervalNotification(string title, string body, int hours)
        {
            iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(hours, 0, 0),
                Repeats = false,
            };

            iOSNotification notification = new iOSNotification
            {
                Identifier = "PlayTime_Alarm_" + Guid.NewGuid(),
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge,
                CategoryIdentifier = "EveryTime",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        #endregion

        #region CALENDAR

        private void SendCalendarNotification(string title, string body, int hour, int minute)
        {
            iOSNotificationCalendarTrigger calendarTrigger = new iOSNotificationCalendarTrigger
            {
                Hour = hour,
                Minute = minute,
                Repeats = true,
            };

            iOSNotification notification = new iOSNotification
            {
                Identifier = "PlayTime_Alarm_" + Guid.NewGuid(),
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge,
                CategoryIdentifier = "EveryTime",
                ThreadIdentifier = "thread1",
                Trigger = calendarTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        #endregion

        #region SEND

        public void SendNotification(string title, string body, int fireTimeHours)
        {
            iOSNotification notification = new iOSNotification
            {
                Identifier = "PlayTime_Alarm_" + Guid.NewGuid(),
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge,
                CategoryIdentifier = "EveryTime",
                ThreadIdentifier = "thread1",
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void ScheduleNotification()
        {
            throw new NotImplementedException();
        }

        #endregion
        
        #region Schedule

        public void ScheduleNotifications()
        {
            if (_notificationsScheduled) return;
            // Last Play To Request
            SendTimeIntervalNotification(
                LocalizeManager.GetLocalString("notificationTitle_24H"),
                LocalizeManager.GetLocalString("notificationBody_24H"),
                24
            );

            // Every Day To Request at 12:00
            SendCalendarNotification(
                LocalizeManager.GetLocalString("notificationTitle_12H"),
                LocalizeManager.GetLocalString("notificationBody_12H"),
                12,
                0
            );

            // Every Day To Request at 20:00
            SendCalendarNotification(
                LocalizeManager.GetLocalString("notificationTitle_20H"),
                LocalizeManager.GetLocalString("notificationBody_20H"),
                20,
                0
            );
            _notificationsScheduled = true;
        }

    #endregion
    }
#endif
}
