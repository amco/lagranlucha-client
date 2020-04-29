using UnityEngine;
using UnityEngine.UI;

using Zenject;
using Newtonsoft.Json;
using CFLFramework.Warnings;
using CFLFramework.API;

namespace CFLFramework.Friends
{
    public class FriendsExample : MonoBehaviour
    {
        #region FIELDS

        private const string InternetConnectionErrorMessage = "Please verify your internet connection";

        [Inject] private WarningsManager warningsManager = null;
        [Inject] private APIManager apiManager = null;
        [Inject] private FriendsManager friendsManager = null;

        [Header("TEXTS")]
        [SerializeField] private Text userIdText = null;
        [SerializeField] private Text responseText = null;

        [Header("INPUT")]
        [SerializeField] private InputField inputField = null;

        [Header("BUTTONS")]
        [SerializeField] private Button searchUserButton = null;
        [SerializeField] private Button getFriendsButton = null;
        [SerializeField] private Button deleteFriendButton = null;
        [SerializeField] private Button getFriendRequestsButton = null;
        [SerializeField] private Button sendFriendRequestButton = null;
        [SerializeField] private Button acceptFriendButton = null;
        [SerializeField] private Button rejectFriendButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            searchUserButton.onClick.AddListener(SearchUser);
            getFriendsButton.onClick.AddListener(GetFriends);
            deleteFriendButton.onClick.AddListener(DeleteFriend);
            getFriendRequestsButton.onClick.AddListener(GetFriendRequests);
            sendFriendRequestButton.onClick.AddListener(SendFriendRequest);
            acceptFriendButton.onClick.AddListener(AcceptFriend);
            rejectFriendButton.onClick.AddListener(RejectFriend);
        }

        private void Update()
        {
            IsLoggedIn();
        }

        private void IsLoggedIn()
        {
            userIdText.text = string.Format("User Id: {0}", apiManager.UserId);
        }

        private string GetResponseError(WebRequestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.CouldNotConnectToHost:
                    return InternetConnectionErrorMessage;
                case HttpStatusCode.Undefined:
                default:
                    return string.Join("\n", response.Errors.Messages());
            }
        }

        private void EraseInputField()
        {
            inputField.text = "";
        }

        private void OnSuccess(WebRequestResponse response, string message, object data = null)
        {
            EraseInputField();
            responseText.text = JsonConvert.SerializeObject(data, Formatting.Indented);

            if (response.Succeeded)
                warningsManager.ShowWarning(message);
            else
                warningsManager.ShowWarning(GetResponseError(response));
        }

        private void SearchUser()
        {
            friendsManager.SearchUsers(inputField.text, webResponse => OnSuccess(webResponse, "Users searched", friendsManager.FoundUsers));
        }

        private void GetFriends()
        {
            friendsManager.GetFriends(webResponse => OnSuccess(webResponse, "Friends aqcuired", friendsManager.CurrentFriends));
        }

        private void GetFriendRequests()
        {
            friendsManager.GetFriendRequests(webResponse => OnSuccess(webResponse, "Friends requests aqcuired", friendsManager.CurrentFriendRequests));
        }

        private void DeleteFriend()
        {
            friendsManager.DeleteFriend(int.Parse(inputField.text), webResponse => OnSuccess(webResponse, "Friend deleted", friendsManager.CurrentFriends));
        }

        private void SendFriendRequest()
        {
            friendsManager.SendFriendRequest(int.Parse(inputField.text), webResponse => OnSuccess(webResponse, "Friend request sent"));
        }

        private void AcceptFriend()
        {
            friendsManager.AcceptFriend(int.Parse(inputField.text), webResponse => OnSuccess(webResponse, "Friend added", friendsManager.CurrentFriends));
        }

        private void RejectFriend()
        {
            friendsManager.RejectFriend(int.Parse(inputField.text), webResponse => OnSuccess(webResponse, "Friend rejected", friendsManager.CurrentFriendRequests));
        }

        #endregion
    }
}
