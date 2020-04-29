using System;
using UnityEngine;

using Zenject;

namespace CFLFramework.Notifications
{
    public class ComeBackNotifications : MonoBehaviour
    {
        #region FIELDS

        [Inject] private NotificationsManager notificationsManager = null;

        [SerializeField] private Notification[] notifications = null;
        [SerializeField] [Range(1, 30)] private int days = 1;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            SetNotification();
            notificationsManager.onEnable += SetNotification;
        }

        private void OnDestroy()
        {
            notificationsManager.onEnable -= SetNotification;
        }

        private void SetNotification()
        {
            Notification notification = notifications[UnityEngine.Random.Range(0, notifications.Length)];
            notificationsManager.ScheduleLocalNotification(TimeSpan.FromDays(days), notification);
        }

        #endregion
    }
}
