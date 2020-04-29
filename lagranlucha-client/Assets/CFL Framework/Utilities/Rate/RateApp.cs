using UnityEngine;
using UnityEngine.UI;

using EasyMobile;

namespace CFLFramework.Utilities.Rate
{
    public class RateApp : MonoBehaviour
    {
        #region FIELDS

        private const string StoreGooglePlayLink = "market://details?id=";
        private const string StoreAppleLink = "itms://apps.apple.com/app/apple-store/id";

        [Header("COMPONENTS")]
        [SerializeField] private Button rateAppButton = null;
        [SerializeField] private string appleId = "";

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            rateAppButton.onClick.AddListener(ShowRateAppDialog);
        }

        private void ShowRateAppDialog()
        {
            if (!StoreReview.CanRequestRating())
            {
#if UNITY_IOS
                UnityEngine.Application.OpenURL(StoreAppleLink + appleId);
#else
                UnityEngine.Application.OpenURL(StoreGooglePlayLink + UnityEngine.Application.identifier);
#endif
                return;
            }

            StoreReview.RequestRating();
        }

        #endregion
    }
}
