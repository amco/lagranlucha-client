using UnityEngine;
using UnityEngine.Events;

using EasyMobile;
using Zenject;

using CFLFramework.Data;
using CFLFramework.Warnings;

namespace CFLFramework.Store
{
    public class StoreManager : MonoBehaviour
    {
        #region FIELDS

        private const string BannersOnlyShowOnBuildsMessage = "Banners are only shown on builds.";
        private const float DPIConversionConstant = 160f;
        private const string StorePreferencessKey = "store_preferences";

        private static readonly string[] RemovesAdsKeys = { StorePreferencessKey, "ads_removed" };

        [Inject] private DataManager dataManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("BANNER CONFIGURATIONS")]
        [SerializeField] private BannerAdPosition bannerPosition = default(BannerAdPosition);

        [Header("CONFIGURATIONS")]
        [SerializeField] private bool testMode = false;

        #endregion

        #region EVENTS

        public event UnityAction<bool> onBannersRemoved;

        #endregion

        #region PROPERTIES

        public bool InAppPurchasedReady { get => InAppPurchasing.IsInitialized(); }
        public bool RewardedAdsReady { get => Advertising.IsRewardedAdReady(); }
        public bool RemovedAds { get => dataManager.GetData<bool>(RemovesAdsKeys, false); }
        public float BannerHeight { get => Mathf.RoundToInt(BannerAdSize.Banner.Height * Screen.dpi / DPIConversionConstant); }
        public bool TestMode { get => testMode; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            Advertising.GrantDataPrivacyConsent();
            InAppPurchasing.InitializePurchasing();
        }

        public void ShowBanner()
        {
            if (RemovedAds)
                return;
#if UNITY_EDITOR
            warningsManager.ShowWarning(BannersOnlyShowOnBuildsMessage);
#endif
            Advertising.ShowBannerAd(bannerPosition);
        }

        public void ShowInterstitial()
        {
            if (RemovedAds)
                return;

            Advertising.ShowInterstitialAd();
        }

        public void HideBanner()
        {
            Advertising.HideBannerAd();
        }

        public void RemoveAds()
        {
            dataManager.SetData(RemovesAdsKeys, true);
            HideBanner();
            onBannersRemoved?.Invoke(true);
        }

        public void ResetStoreData()
        {
            dataManager.SetData(RemovesAdsKeys, false);
            onBannersRemoved?.Invoke(false);
        }

        #endregion
    }
}
