using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Store
{
    public class StoreExample : MonoBehaviour
    {
        #region FIELDS

        [Inject] private StoreManager storeManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button showBannerButton = null;
        [SerializeField] private Button hideBannerButton = null;
        [SerializeField] private Button resetBannerDataButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            showBannerButton.onClick.AddListener(storeManager.ShowBanner);
            hideBannerButton.onClick.AddListener(storeManager.HideBanner);
            resetBannerDataButton.onClick.AddListener(storeManager.ResetStoreData);
        }

        #endregion
    }
}
