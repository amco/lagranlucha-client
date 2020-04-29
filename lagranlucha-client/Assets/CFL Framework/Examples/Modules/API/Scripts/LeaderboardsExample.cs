using UnityEngine;
using UnityEngine.UI;

using Zenject;
using Newtonsoft.Json;
using CFLFramework.Warnings;
using CFLFramework.API;

namespace CFLFramework.Leaderboards
{
    public class LeaderboardsExample : MonoBehaviour
    {
        #region FIELDS

        private const string InternetConnectionErrorMessage = "Please verify your internet connection";

        [Inject] private WarningsManager warningsManager = null;
        [Inject] private APIManager apiManager = null;
        [Inject] private LeaderboardsManager leaderboardsManager = null;

        [Header("TEXTS")]
        [SerializeField] private Text userIdText = null;
        [SerializeField] private Text responseText = null;

        [Header("INPUT")]
        [SerializeField] private InputField inputField = null;

        [Header("BUTTONS")]
        [SerializeField] private Button getLeaderboardButton = null;
        [SerializeField] private Button getFriendsLeaderboardButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            getLeaderboardButton.onClick.AddListener(GetLeaderboard);
            getFriendsLeaderboardButton.onClick.AddListener(GetFriendsLeaderboard);
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

        private void GetLeaderboard()
        {
            leaderboardsManager.GetLeaderboard(inputField.text, webResponse => OnSuccess(webResponse, "Friends acquired", leaderboardsManager.CurrentLeaderboard));
        }

        private void GetFriendsLeaderboard()
        {
            leaderboardsManager.GetFriendsLeaderboard(inputField.text, webResponse => OnSuccess(webResponse, "Friends leaderboard acquired", leaderboardsManager.CurrentFriendsLeaderboard));
        }

        #endregion
    }
}
