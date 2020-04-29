using UnityEngine;

using EasyMobile;

namespace CFLFramework.Notifications
{
    [CreateAssetMenu(menuName = ScriptableCreationRoute)]
    public class Notification : ScriptableObject
    {
        #region FIELDS

        private const string ScriptableCreationRoute = "CFL Framework/Notifications/New Notification";
        private const string NotificationsRoute = "Notifications/";

        [SerializeField] private string id = "";
        [SerializeField] private string title = "";
        [SerializeField] private string body = "";
        [SerializeField] private bool localize = false;

        #endregion

        #region PROPERTIES

        public string PlayerPrefKey { get => NotificationsRoute + id; }

        public string NotificationID
        {
            get => PlayerPrefs.GetString(PlayerPrefKey, string.Empty);
            set => PlayerPrefs.SetString(PlayerPrefKey, value);
        }

        public virtual NotificationContent Content
        {
            get
            {
                NotificationContent content = new NotificationContent();
                content.title = title;
                content.body = body;
                return content;
            }
        }

        public bool Localize { get => localize; }

        #endregion
    }
}
