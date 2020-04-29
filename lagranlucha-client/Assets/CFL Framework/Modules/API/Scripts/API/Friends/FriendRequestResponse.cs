using CFLFramework.Data;

namespace CFLFramework.API
{
    public class FriendRequestResponse
    {
        #region FIELDS

        private const string BackendClassName = "friend_request";

        #endregion

        #region PROPERTIES

        public string Id { get; set; }
        public string Type { get; set; } = BackendClassName;
        public UserData User { get; set; }

        #endregion
    }
}
