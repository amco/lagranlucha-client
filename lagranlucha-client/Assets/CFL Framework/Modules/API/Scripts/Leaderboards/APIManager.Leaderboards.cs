﻿using UnityEngine.Events;

namespace CFLFramework.API
{
    public partial class APIManager
    {
        #region BEHAVIORS

        internal void GetLeaderboard(string offset, bool isFriends, UnityAction<WebRequestResponse> response = null)
        {
            RequestContent requestContent = new RequestContent(RequestType.Get, Endpoint.Leaderboard);
            requestContent.Parameters.Add(PredicateKey.Include, PredicateValue.LeaderboardEntries);
            requestContent.Parameters.Add(PredicateKey.FieldUser, PredicateValue.LeaderboardAvatar);
            requestContent.Parameters.Add(PredicateKey.LeaderboardOffset, offset);
            if (isFriends)
                requestContent.Parameters.Add(PredicateKey.LeaderboardFriends, isFriends.ToString());

            requester.SendRequest(this, requestContent, response);
        }

        #endregion      
    }
}
