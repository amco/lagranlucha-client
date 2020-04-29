using UnityEngine;

using Zenject;
using CFLFramework.Analytics;

namespace CFLFramework.Store
{
    [RequireComponent(typeof(StoreReward), typeof(StoreElement))]
    public abstract class PurchaseAnalytics : MonoBehaviour
    {
        #region FIELDS

        [Inject] protected AnalyticsManager analyticsManager = null;

        private StoreElement storeElement = null;
        private StoreReward storeReward = null;

        #endregion

        #region PROPERTIES

        protected abstract string PurchaseRequestedAnalyticName { get; }
        protected abstract string PurchaseCompletedAnalyticName { get; }

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            storeElement = GetComponent<StoreElement>();
            storeReward = GetComponent<StoreReward>();
            storeElement.onPurchaseRequested += SendPurchaseRequestedAnalytics;
            storeReward.onPurchaseCompleted += SendPurchaseCompletedAnalytics;
        }

        private void OnDestroy()
        {
            storeElement.onPurchaseRequested -= SendPurchaseRequestedAnalytics;
            storeReward.onPurchaseCompleted -= SendPurchaseCompletedAnalytics;
        }

        private void SendPurchaseRequestedAnalytics()
        {
            if (string.IsNullOrEmpty(PurchaseRequestedAnalyticName))
                return;

            analyticsManager.Send(PurchaseRequestedAnalyticName);
        }

        private void SendPurchaseCompletedAnalytics()
        {
            if (string.IsNullOrEmpty(PurchaseCompletedAnalyticName))
                return;

            analyticsManager.Send(PurchaseCompletedAnalyticName, true, true);
        }

        #endregion
    }
}
