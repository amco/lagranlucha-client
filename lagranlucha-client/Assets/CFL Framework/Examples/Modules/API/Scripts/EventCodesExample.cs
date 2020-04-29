using UnityEngine;
using UnityEngine.UI;

using Zenject;
using Newtonsoft.Json;
using CFLFramework.API;
using CFLFramework.Warnings;

namespace CFLFramework.EventCodes
{
    public class EventCodesExample : MonoBehaviour
    {
        #region FIELDS

        private const string InternetConnectionErrorMessage = "Please verify your internet connection";

        [Inject] private WarningsManager warningsManager = null;
        [Inject] private APIManager apiManager = null;
        [Inject] private EventCodesManager eventCodesManager = null;

        [Header("TEXTS")]
        [SerializeField] private Text isLoggedText = null;
        [SerializeField] private Text responseText = null;

        [Header("INPUT")]
        [SerializeField] private InputField inputField = null;

        [Header("BUTTONS")]
        [SerializeField] private Button sendCodeButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            sendCodeButton.onClick.AddListener(SendCode);
        }

        private void Update()
        {
            IsLoggedIn();
        }

        private void IsLoggedIn()
        {
            isLoggedText.text = string.Format("Logged: {0}", apiManager.IsLoggedIn);
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

            if (response.Succeeded)
                warningsManager.ShowWarning(message);
            else
                warningsManager.ShowWarning(GetResponseError(response));
        }

        private void OnCodeAccepted(EventCode eventCode)
        {
            responseText.text = JsonConvert.SerializeObject(eventCode, Formatting.Indented);
        }

        private void SendCode()
        {
            eventCodesManager.SendCode(inputField.text, OnCodeAccepted, webResponse => OnSuccess(webResponse, "Code sent"));
        }

        #endregion
    }
}
