using UnityEngine;

using Zenject;
using EasyMobile;

namespace CFLFramework.Store
{
    public class StoreAdRewardedElement : StoreElement
    {
        #region FIELDS

        [Inject] private StoreManager storeManager = null;

        #endregion

        #region BEHAVIORS

        protected override bool IsAvailable()
        {
            if (Application.isEditor)
                return true;

            return storeManager.RewardedAdsReady;
        }

        protected override void Buy()
        {
            if (Application.isEditor || storeManager.TestMode)
            {
                storeReward.PurchaseCompleted();
                return;
            }

            Advertising.RewardedAdSkipped += RewardedAdSkippedHandler;
            Advertising.RewardedAdCompleted += RewardedAdCompletedHandler;
            Advertising.ShowRewardedAd();
        }

        private void RewardedAdCompletedHandler(RewardedAdNetwork network, AdPlacement location)
        {
            Advertising.RewardedAdSkipped -= RewardedAdSkippedHandler;
            Advertising.RewardedAdCompleted -= RewardedAdCompletedHandler;
            storeReward.PurchaseCompleted();
        }

        private void RewardedAdSkippedHandler(RewardedAdNetwork network, AdPlacement location)
        {
            Advertising.RewardedAdSkipped -= RewardedAdSkippedHandler;
            Advertising.RewardedAdCompleted -= RewardedAdCompletedHandler;
            ShowNotAvailableWindow();
        }

        #endregion
    }
}
