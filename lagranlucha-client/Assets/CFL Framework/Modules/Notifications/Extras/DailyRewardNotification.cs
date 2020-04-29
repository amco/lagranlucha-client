using System;
using UnityEngine;

using Zenject;
using CFLFramework.DailyRewards;

namespace CFLFramework.Notifications
{
    public class DailyRewardNotification : MonoBehaviour
    {
        #region FIELDS

        [Inject] private NotificationsManager notificationsManager = null;
        [Inject] private DailyRewardsManager dailyRewardsManager = null;

        [SerializeField] private Notification notification = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            notificationsManager.onEnable += EnabledNotifications;
            dailyRewardsManager.onCalculatedNextDailyTime += ClaimedReward;
        }

        private void OnDestroy()
        {
            notificationsManager.onEnable -= EnabledNotifications;
            dailyRewardsManager.onCalculatedNextDailyTime -= ClaimedReward;
        }

        private void ClaimedReward(TimeSpan timeForNotification)
        {
            SetNotification(timeForNotification);
        }

        private void EnabledNotifications()
        {
            SetNotification(dailyRewardsManager.GetRemainingTime());
        }

        private void SetNotification(TimeSpan timeSpan)
        {
            notificationsManager.ScheduleLocalNotification(timeSpan, notification);
        }

        #endregion
    }
}
