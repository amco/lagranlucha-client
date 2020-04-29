using System.Collections.Generic;
using UnityEngine;

using Zenject;
using CFLFramework.Analytics;

namespace CFLFramework.Store
{
    [RequireComponent(typeof(StoreReward))]
    public abstract class FirstTimePurchaseAnalytics : MonoBehaviour
    {
        #region FIELDS

        [Inject] protected AnalyticsManager analyticsManager = null;

        private StoreReward storeReward = null;

        #endregion

        #region PROPERTIES

        protected abstract string FirstTimePurchaseAnalyticName { get; }
        protected abstract Dictionary<string, object> Data { get; }
        public bool HavePurchasedBefore
        {
            get => PlayerPrefs.HasKey(FirstTimePurchaseAnalyticName);
            set
            {
                if (value)
                    PlayerPrefs.SetString(FirstTimePurchaseAnalyticName, string.Empty);
                else
                    PlayerPrefs.DeleteKey(FirstTimePurchaseAnalyticName);
            }
        }

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            storeReward = GetComponent<StoreReward>();
            storeReward.onPurchaseCompleted += SendAnalytics;
        }

        private void OnDestroy()
        {
            storeReward.onPurchaseCompleted -= SendAnalytics;
        }

        private void SendAnalytics()
        {
            if (HavePurchasedBefore)
                return;

            analyticsManager.Send(FirstTimePurchaseAnalyticName, true, true, true, Data);
            HavePurchasedBefore = true;
        }

        #endregion
    }
}
