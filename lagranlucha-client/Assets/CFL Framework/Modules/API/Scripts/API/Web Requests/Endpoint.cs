namespace CFLFramework.API
{
    public static class Endpoint
    {
        #region FIELDS

        public const string Users = "/users";
        public const string UsersId = "/users/";
        public const string Friends = "/friends";
        public const string FriendsId = "/friends/";
        public const string FriendRequestsId = "/friend_requests/";
        public const string FriendRequestsAccept = "/accepts";
        public const string FriendRequestsReject = "/rejects";
        public const string FriendRequests = "/friend_requests";
        public const string RecoveryPassword = "/users/password";
        public const string Leaderboard = "/leaderboards/users";
        public const string LeaderboardCountries = "/leaderboards/countries";
        public const string Gifts = "/gifts";
        public const string GiftsId = "/gifts/";
        public const string Redeem = "/redeems";
        public const string EventCodes = "/event_codes/";
        public const string GoogleLogin = "/users/auth/google/callback/";
        public const string AppleLogin = "/users/auth/apple/callback/";
        public const string GoogleUnlink = "/users/{0}/unlinks/google";
        public const string AppleUnlink = "/users/{0}/unlinks/apple";

        #endregion
    }
}
