using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Zenject;
using Newtonsoft.Json;
using CFLFramework.Warnings;
using CFLFramework.API;

namespace CFLFramework.Gifts
{
    public class GiftsExample : MonoBehaviour
    {
        #region FIELDS

        private const string InternetConnectionErrorMessage = "Please verify your internet connection";

        private static readonly string[] GiftTestKeys = new string[] { "collectibles", "coins", "gold" };

        [Inject] private WarningsManager warningsManager = null;
        [Inject] private APIManager apiManager = null;
        [Inject] private GiftsManager giftsManager = null;

        [Header("TEXTS")]
        [SerializeField] private Text loggedText = null;
        [SerializeField] private Text responseText = null;

        [Header("INPUT")]
        [SerializeField] private InputField inputField = null;

        [Header("BUTTONS")]
        [SerializeField] private Button getGiftsButton = null;
        [SerializeField] private Button sendGiftButton = null;
        [SerializeField] private Button redeemNextGiftButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            getGiftsButton.onClick.AddListener(GetGifts);
            sendGiftButton.onClick.AddListener(SendGift);
            redeemNextGiftButton.onClick.AddListener(RedeemNextGift);
        }

        private void Update()
        {
            IsLoggedIn();
        }

        private void IsLoggedIn()
        {
            loggedText.text = string.Format("User Id: {0}", apiManager.UserId);
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
            responseText.text = JsonConvert.SerializeObject(giftsManager.CurrentGifts, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });

            if (response.Succeeded)
                warningsManager.ShowWarning(message);
            else
                warningsManager.ShowWarning(GetResponseError(response));
        }

        private void GetGifts()
        {
            giftsManager.GetGifts(response => OnSuccess(response, "Gifts acquired"));
        }

        private void SendGift()
        {
            giftsManager.SendGift(int.Parse(inputField.text), GiftTestKeys, 5, response => OnSuccess(response, "Gift sent"));
        }

        private void RedeemNextGift()
        {
            if (giftsManager.CurrentGifts.Length == 0)
            {
                warningsManager.ShowWarning("No gifts available");
                return;
            }

            giftsManager.RedeemGift(giftsManager.CurrentGifts.First(), response => OnSuccess(response, "Gift redeemed"));
        }

        #endregion
    }
}
