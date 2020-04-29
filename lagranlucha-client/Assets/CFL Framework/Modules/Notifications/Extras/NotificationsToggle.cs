using UnityEngine;

using CFLFramework.Utilities;
using Zenject;

namespace CFLFramework.Notifications
{
    public class NotificationsToggle : MonoBehaviour
    {
        #region FIELDS

        [Inject] private NotificationsManager notificationsManager = null;

        [SerializeField] private Toggle toggle = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            toggle.onChange += SetNotificationsState;
        }

        private void OnDestroy()
        {
            toggle.onChange -= SetNotificationsState;
        }

        private void OnEnable()
        {
            toggle.Initialize(notificationsManager.Enabled);
        }

        private void SetNotificationsState(bool status)
        {
            notificationsManager.Enabled = status;
        }

        #endregion
    }
}
