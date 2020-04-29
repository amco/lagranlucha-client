namespace CFLFramework.API
{
    public static class PredicateKey
    {
        #region FIELDS
        public const string Include = "include";
        public const string FieldUser = "fields[user]";
        public const string FieldFriend = "fields[friend]";
        public const string FieldGift = "fields[gift]";
        public const string FieldSender = "fields[sender]";
        public const string FieldRecipient = "jsonb_fields[recipient]";
        public const string LeaderboardOffset = "leaderboard[offset]";
        public const string LeaderboardFriends = "leaderboard[friends]";
        public const string QueryIdEqual = "q[id_eq]";
        public const string QueryStatusEqual = "q[status_eq]";
        public const string QueryUsernameContains = "q[username_cont]";
        public const string QueryUsernameNotEqual = "q[username_not_eq]";
        public const string QueryUserUsernameNotEqual = "q[user_username_not_eq]";
        public const string QuerySenderUsernameNotEqual = "q[sender_username_not_eq]";
        public const string QueryIdNotEqual = "q[id_not_eq]";
        public const string QueryFriendIdEqual = "q[friend_id_eq]";
        public const string QueryRecipientIdEqual = "q[recipient_id_eq]";
        public const string QueryFriendUsernameNotEqual = "q[friends_username_not_eq]";
        #endregion
    }
}
