using System.Collections.Generic;
using CFLFramework.Data;

namespace CFLFramework.API
{
    public class LeaderboardData
    {
        #region FIELDS

        private const string BackendClassName = "users_leaderboard";

        #endregion

        #region PROPERTIES

        public string Id { get; set; }
        public string Type { get; set; } = BackendClassName;
        public UserData Entry { get; set; }
        public List<UserData> Entries { get; set; }

        #endregion
    }
}
