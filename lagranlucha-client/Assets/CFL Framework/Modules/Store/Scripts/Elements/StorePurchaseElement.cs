using UnityEngine;

using Zenject;
using EasyMobile;
using CFLFramework.Warnings;

namespace CFLFramework.Store
{
    public class StorePurchaseElement : StoreElement
    {
        #region FIELDS

        private const string PurchaseFailedMessage = "The purchase has failed.";

        [Inject] private StoreManager storeManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private string productName = null;

        #endregion

        #region PROPERTIES

#if EM_ADMOB
        private bool IsProductNameRegistered { get => InAppPurchasing.GetProduct(productName) != null; }
#endif

        #endregion

        #region BEHAVIORS

        private void OnEnable()
        {
            InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
            InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
        }

        private void OnDisable()
        {
            InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
            InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
        }

        protected override bool IsAvailable()
        {
            return storeManager.InAppPurchasedReady;
        }

        protected override void Buy()
        {
#if EM_ADMOB

            if (!IsProductNameRegistered && (Application.isEditor || storeManager.TestMode))
            {
                ShowNotAvailableWindow();
                return;
            }
#endif

            InAppPurchasing.Purchase(productName);
        }

        private void PurchaseCompletedHandler(IAPProduct product)
        {
            if (productName != product.Name)
                return;

            storeReward.PurchaseCompleted();
        }

        private void PurchaseFailedHandler(IAPProduct product)
        {
            warningsManager.ShowWarning(PurchaseFailedMessage, productName);
        }

        #endregion
    }
}
