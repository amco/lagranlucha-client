using UnityEngine;

using TMPro;
using Zenject;
using EasyMobile;

namespace CFLFramework.Store
{
    public class StorePurchaseElementPriceUI : MonoBehaviour
    {
        #region FIELDS

        private const string UnknownPrice = "???";

        [Inject] private StoreManager storeManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private TMP_Text productPriceUI = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private string productName = null;

        #endregion

        #region PROPERTIES

#if EM_ADMOB
        private string Price { get => InAppPurchasing.GetProductLocalizedData(productName).localizedPriceString; }
#endif

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            if (storeManager.TestMode)
            {
                SetText(UnknownPrice);
                return;
            }

#if EM_ADMOB
            if (InAppPurchasing.IsInitialized())
                SetText(Price);
            else
                InAppPurchasing.InitializeSucceeded += InitializedSuccessfuly;
#endif
        }

        private void InitializedSuccessfuly()
        {
#if EM_ADMOB
            SetText(Price);
            InAppPurchasing.InitializeSucceeded -= InitializedSuccessfuly;
#endif
        }

        private void SetText(string price)
        {
            productPriceUI.text = price;
        }

        #endregion
    }
}
