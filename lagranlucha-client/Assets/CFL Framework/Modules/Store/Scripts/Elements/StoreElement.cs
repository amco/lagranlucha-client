using System;
using UnityEngine;
using UnityEngine.UI;

namespace CFLFramework.Store
{
    public abstract class StoreElement : MonoBehaviour
    {
        #region FIELDS

        [Header("COMPONENTS")]
        [SerializeField] private StoreNotAvailableWindow storeNotAvailableWindow = null;
        [SerializeField] private Button storeElementButton = null;

        protected StoreReward storeReward = null;

        #endregion

        #region EVENTS

        public event Action onPurchaseRequested;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            storeElementButton.onClick.AddListener(CheckAvailability);
            storeReward = GetComponentInChildren<StoreReward>();
        }

        private void CheckAvailability()
        {
            if (IsAvailable())
            {
                onPurchaseRequested?.Invoke();
                Buy();
            }
            else
            {
                ShowNotAvailableWindow();
            }
        }

        protected void ShowNotAvailableWindow()
        {
            if (storeNotAvailableWindow != null)
                Instantiate(storeNotAvailableWindow);
        }

        protected abstract bool IsAvailable();
        protected abstract void Buy();

        #endregion
    }
}
