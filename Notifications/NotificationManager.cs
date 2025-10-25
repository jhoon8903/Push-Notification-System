using Firebase.Messaging;
using UnityEngine;
using System;
using Projects.Scripts.Plugin.Notifications;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace Scripts.Plugin.Notifications
{
    public static class NotificationManager
    {
        private const string DontAskAgain = "DontAskNotificationPermission";
        private static Projects.Scripts.Plugin.Notifications.INotification _notification;

        public static void InitializeNotification()
        {
            if (PlayerPrefs.HasKey(DontAskAgain)) return;
#if UNITY_ANDROID
            _notification = new AosNotification();
            _notification.RequestAuthorization();
            _notification.RegisterNotification();
#elif UNITY_IOS
            _notification = new IOSNotification();
            _notification.RequestAuthorization();
#endif
            AppStateEventNotifier.AppStateChanged += InOut;
        }

        private static void InOut(AppState obj)
        {
            if (obj == AppState.Background)
            {
#if UNITY_ANDROID
                if (PlayerPrefs.HasKey(DontAskAgain)) return;
                if (!AndroidNotificationCenter.Initialize()) return;
                AndroidNotificationCenter.CancelAllNotifications();
                _notification?.ScheduleNotification();
#elif UNITY_IOS
                iOSNotificationSettings notification = iOSNotificationCenter.GetNotificationSettings();
                if (notification.NotificationCenterSetting != NotificationSetting.Enabled) return;
                iOSNotificationCenter.RemoveAllScheduledNotifications();
                _notification?.ScheduleNotification();
#endif
            }
        }

        public static void OnFirebaseMessaging(object sender, MessageReceivedEventArgs e)
        {
            if (PlayerPrefs.HasKey(DontAskAgain)) return;
            try
            {
                string title = e.Message.Notification?.Title ?? "New Message";
                string body = e.Message.Notification?.Body ?? "You have a new notification";
                _notification?.SendNotification(title, body, 0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing Firebase message: {ex.Message}");
            }
            finally
            {
                e.Message = null;
            }
        }
    }
}