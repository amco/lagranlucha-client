using UnityEngine;
using UnityEngine.Events;

using Zenject;
using CFLFramework.API;

namespace CFLFramework.Leaderboards
{
    public class LeaderboardsManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] private APIManager apiManager = null;

        private LeaderboardData currentLeaderboard = null;
        private LeaderboardData currentFriendsLeaderboard = null;

        #endregion

        #region PROPERTIES

        public LeaderboardData CurrentLeaderboard { get => currentLeaderboard; }
        public LeaderboardData CurrentFriendsLeaderboard { get => currentFriendsLeaderboard; }

        #endregion

        #region BEHAVIORS

        public void GetLeaderboard(string offset, UnityAction<WebRequestResponse> response)
        {
            apiManager.GetLeaderboard(offset, false, (webResponse => PopulateLeaderboard(ref currentLeaderboard, webResponse)) + response);
        }

        public void GetFriendsLeaderboard(string offset, UnityAction<WebRequestResponse> response)
        {
            apiManager.GetLeaderboard(offset, true, (webResponse => PopulateLeaderboard(ref currentFriendsLeaderboard, webResponse)) + response);
        }

        private void PopulateLeaderboard(ref LeaderboardData leaderboard, WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            leaderboard = response.Response<LeaderboardData>();
        }

        #endregion
    }
}
