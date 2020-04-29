using System;
using UnityEngine;

using Zenject;
using CFLFramework.Localization;

namespace CFLFramework.Notifications
{
    public class NotificationsManager : MonoBehaviour
    {
        #region FIELDS

        private const int On = 1;
        private const int Off = 0;
        private const string NotificationsEnabledRoute = "Notifications/Enabled";

        [Inject] private LocalizationManager localizationManager = null;

        #endregion

        #region EVENTS

        public event Action onEnable = null;

        #endregion

        #region PROPERTIES

        public bool Enabled
        {
            get
            {
                return PlayerPrefs.GetInt(NotificationsEnabledRoute, On) == On;
            }
            set
            {
                if (value == Enabled)
                    return;

                if (value)
                {
                    PlayerPrefs.SetInt(NotificationsEnabledRoute, On);
                    onEnable?.Invoke();
                }
                else
                {
                    PlayerPrefs.SetInt(NotificationsEnabledRoute, Off);
                    CancelAllNotifications();
                }
            }
        }

        #endregion


        #region BEHAVIORS

        private void Start()
        {
            EasyMobile.Notifications.Init();
            EasyMobile.Notifications.GrantDataPrivacyConsent();
        }

        public void ScheduleLocalNotification(DateTime triggerDate, Notification notification)
        {
            CancelLocalNotification(notification);
            LocalizeNotification(notification);
            notification.NotificationID = EasyMobile.Notifications.ScheduleLocalNotification(triggerDate, notification.Content);
        }

        public void ScheduleLocalNotification(TimeSpan delay, Notification notification)
        {
            CancelLocalNotification(notification);
            LocalizeNotification(notification);
            notification.NotificationID = EasyMobile.Notifications.ScheduleLocalNotification(delay, notification.Content);
        }

        private void LocalizeNotification(Notification notification)
        {
            if (!notification.Localize)
                return;

            notification.Content.title = localizationManager.GetTranslation(notification.Content.title);
            notification.Content.body = localizationManager.GetTranslation(notification.Content.body);
        }

        private void CancelLocalNotification(Notification notification)
        {
            if (string.IsNullOrEmpty(notification.NotificationID))
                return;

            EasyMobile.Notifications.CancelPendingLocalNotification(notification.NotificationID);
        }

        private void CancelAllNotifications()
        {
            EasyMobile.Notifications.CancelAllPendingLocalNotifications();
        }

        #endregion
    }
}
