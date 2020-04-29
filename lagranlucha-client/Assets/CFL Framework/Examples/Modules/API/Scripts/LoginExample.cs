using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Warnings;
using CFLFramework.Data;

namespace CFLFramework.API
{
    public class LoginExample : MonoBehaviour
    {
        #region FIELDS

        protected const string InternetConnectionErrorMessage = "Please verify your internet connection";

        [Inject] private WarningsManager warningsManager = null;
        [Inject] private APIManager apiManager = null;
        [Inject] private DataManager dataManager = null;

        [Header("TEXTS")]
        [SerializeField] private Text loggedText = null;
        [SerializeField] private Text emailLinkedText = null;
        [SerializeField] private Text usernameText = null;

        [Header("INPUT")]
        [SerializeField] private InputField inputField = null;

        [Header("BUTTONS")]
        [SerializeField] private Button createAccountButton = null;
        [SerializeField] private Button linkEmailButton = null;
        [SerializeField] private Button requestTokenButton = null;
        [SerializeField] private Button insertTokenButton = null;
        [SerializeField] private Button logoutButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            createAccountButton.onClick.AddListener(CreateAccount);
            linkEmailButton.onClick.AddListener(LinkEmail);
            requestTokenButton.onClick.AddListener(RequestToken);
            insertTokenButton.onClick.AddListener(InsertToken);
            logoutButton.onClick.AddListener(LogOut);
        }

        private void Update()
        {
            IsLoggedIn();
            IsEmailLinked();
            ShowMyUsername();
        }

        private void IsLoggedIn()
        {
            loggedText.text = string.Format("Logged: {0}", apiManager.IsLoggedIn);
        }

        private void IsEmailLinked()
        {
            emailLinkedText.text = string.Format("Email Linked: {0}", string.IsNullOrEmpty(dataManager.User.Email) ? "None" : dataManager.User.Email);
        }

        private void ShowMyUsername()
        {
            usernameText.text = string.Format("Username: {0}", string.IsNullOrEmpty(dataManager.User.Username) ? "None" : dataManager.User.Username);
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

        private void OnSuccess(WebRequestResponse response, string message)
        {
            EraseInputField();
            if (response.Succeeded)
                warningsManager.ShowWarning(message);
            else
                warningsManager.ShowWarning(GetResponseError(response));
        }

        private void CreateAccount()
        {
            apiManager.CreateAccount(inputField.text, (response) => OnSuccess(response, "Account created"));
        }

        private void LinkEmail()
        {
            apiManager.LinkEmail(inputField.text, (response) => OnSuccess(response, "Email linked"));
        }

        private void RequestToken()
        {
            apiManager.RequestToken(inputField.text, (response) => OnSuccess(response, "Token requested"));
        }

        private void InsertToken()
        {
            apiManager.SendConfirmationToken(inputField.text, (response) => OnSuccess(response, "Token was correct"));
        }

        private void LogOut()
        {
            apiManager.LogOut();
            warningsManager.ShowWarning("Logged out");
        }

        #endregion
    }
}
