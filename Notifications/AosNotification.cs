using System;
using Projects.Scripts.Localize;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine.Android;

namespace Projects.Scripts.Plugin.Notifications
{
    #if UNITY_ANDROID
    public class AosNotification : INotification
    {
        private bool _notificationsScheduled;
        
        public void RequestAuthorization()
        {
            if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
            }
        }

        public void RegisterNotification()
        {
            AndroidNotificationChannel channel = new AndroidNotificationChannel
            {
                Id = "PlayTime_Alarm",
                Name = "Play Time Alarm",
                Description = "Channel for Play Time notifications",
                Importance = Importance.Default,
                CanBypassDnd = false,
                CanShowBadge = true,
                EnableLights = true,
                EnableVibration = true,
                VibrationPattern = new long[] { 100, 200, 300 },
                LockScreenVisibility = LockScreenVisibility.Public
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        #region INTERVAL

        private void SendInexactTimeIntervalNotification(string title, string text, int fireTimeHours)
        {
            var notification = new AndroidNotification
            {
                Title = title,
                Text = text,
                FireTime = DateTime.Now.AddHours(fireTimeHours),
                SmallIcon = "icon_0",
                LargeIcon = "icon_1"
            };

            AndroidNotificationCenter.SendNotification(notification, "PlayTime_Alarm");
        }

        #endregion

        #region CALENDAR

        private void SendCalendarNotification(string title, string text, int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime fireTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0, DateTimeKind.Local);
            if (fireTime < now)
            {
                fireTime = fireTime.AddDays(1);
            }

            AndroidNotification notification = new AndroidNotification
            {
                Title = title,
                Text = text,
                FireTime = fireTime,
                RepeatInterval = TimeSpan.FromDays(1),
                SmallIcon = "icon_0",
                LargeIcon = "icon_1"
            };

            AndroidNotificationCenter.SendNotification(notification, "PlayTime_Alarm");
        }

        #endregion

        #region SEND

        public void SendNotification(string title, string text, int fireTimeHours)
        {
            AndroidNotification notification = new AndroidNotification
            {
                Title = title,
                Text = text,
                FireTime = DateTime.Now.AddHours(fireTimeHours),
                SmallIcon = "icon_0",
                LargeIcon = "icon_1"
            };
            AndroidNotificationCenter.SendNotification(notification, "Notification!");
        }

        #endregion

        #region Schedule

        public void ScheduleNotification()
        {
            if (_notificationsScheduled) return;

            SendInexactTimeIntervalNotification(
                LocalizeManager.GetLocalString("notificationTitle_24H"),
                LocalizeManager.GetLocalString("notificationBody_24H"),
                24
            );

            SendCalendarNotification(
                LocalizeManager.GetLocalString("notificationTitle_12H"),
                LocalizeManager.GetLocalString("notificationBody_12H"),
                12,
                0
            );

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