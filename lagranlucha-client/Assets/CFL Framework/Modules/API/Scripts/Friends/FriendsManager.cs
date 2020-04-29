using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

using Zenject;
using Newtonsoft.Json;
using CFLFramework.API;
using CFLFramework.Data;

namespace CFLFramework.Friends
{
    public class FriendsManager : MonoBehaviour
    {
        #region FIELDS

        public const string CurrentFriendsKey = "CurrentFriends";
        public const string FriendRequestsKey = "FriendRequests";

        [Inject] private APIManager apiManager = null;

        private List<UserData> currentFriends = new List<UserData>();
        private List<FriendRequest> currentFriendRequests = new List<FriendRequest>();
        private List<UserData> foundUsers = new List<UserData>();

        #endregion

        #region PROPERTIES

        public UserData[] CurrentFriends { get => currentFriends == null ? new UserData[] { } : currentFriends.ToArray(); }
        public FriendRequest[] CurrentFriendRequests { get => currentFriendRequests == null ? new FriendRequest[] { } : currentFriendRequests.ToArray(); }
        public UserData[] FoundUsers { get => foundUsers == null ? new UserData[] { } : foundUsers.ToArray(); }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            LoadLocalFriendsData();
        }

        public void SearchUsers(string userName, UnityAction<WebRequestResponse> response)
        {
            apiManager.SearchUser(userName, (webResponse => PopulateList(ref foundUsers, webResponse)) + response);
        }

        public void GetFriends(UnityAction<WebRequestResponse> response)
        {
            apiManager.GetFriends((webResponse => PopulateList(ref currentFriends, webResponse)) + response);
        }

        public void GetFriendRequests(UnityAction<WebRequestResponse> response)
        {
            apiManager.GetFriendRequests((webResponse => PopulateList(ref currentFriendRequests, webResponse)) + response);
        }

        public void DeleteFriend(int friendId, UnityAction<WebRequestResponse> response)
        {
            apiManager.DeleteFriend(friendId, (webResponse => RemoveFriend(friendId, webResponse)) + response);
        }

        public void SendFriendRequest(int friendId, UnityAction<WebRequestResponse> response)
        {
            apiManager.CreateFriendRequest(friendId, response);
        }

        public void AcceptFriend(int friendId, UnityAction<WebRequestResponse> response)
        {
            apiManager.AcceptFriendRequest(friendId, (webResponse => AddFriend(friendId, webResponse)) + response);
        }

        public void RejectFriend(int friendId, UnityAction<WebRequestResponse> response)
        {
            apiManager.RejectFriendRequest(friendId, (webResponse => RemoveFriendRequest(friendId, webResponse)) + response);
        }

        private void PopulateList<T>(ref List<T> list, WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            list = new List<T>(response.ResponseArray<T>());
            SaveLocalFriendsData();
        }

        private void AddFriend(int friendId, WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            currentFriendRequests.RemoveAll(friendRequest => friendRequest.SenderId == friendId);
            currentFriends.Add(response.Response<UserData>());
            SaveLocalFriendsData();
        }

        private void RemoveFriend(int friendId, WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            currentFriends.RemoveAll(friend => friend.Id == friendId.ToString());
            SaveLocalFriendsData();
        }

        private void RemoveFriendRequest(int friendId, WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            currentFriendRequests.RemoveAll(friendRequest => friendRequest.SenderId == friendId);
            SaveLocalFriendsData();
        }

        private void SaveLocalFriendsData()
        {
            PlayerPrefs.SetString(CurrentFriendsKey, JsonConvert.SerializeObject(currentFriends, Formatting.Indented));
            PlayerPrefs.SetString(FriendRequestsKey, JsonConvert.SerializeObject(currentFriendRequests, Formatting.Indented));
        }

        private void LoadLocalFriendsData()
        {
            currentFriends = JsonConvert.DeserializeObject<List<UserData>>(PlayerPrefs.GetString(CurrentFriendsKey), new GenericConverter());
            currentFriendRequests = JsonConvert.DeserializeObject<List<FriendRequest>>(PlayerPrefs.GetString(FriendRequestsKey), new GenericConverter());
        }

        #endregion
    }
}
