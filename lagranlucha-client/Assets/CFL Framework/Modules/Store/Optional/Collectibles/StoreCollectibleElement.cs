using UnityEngine;

using Zenject;
using CFLFramework.Collectibles;

namespace CFLFramework.Store
{
    public class StoreCollectibleElement : StoreElement
    {
        #region FIELDS

        private const int MinAmount = 0;
        private const int MaxAmount = 999;
        private const string AmountFormat = "{0}";

        [Inject] private StoreManager storeManager = null;
        [Inject] private CollectiblesManager collectiblesManager = null;

        [Header("CUSTOM COMPONENTS")]
        [SerializeField] private StoreConfirmationWindow storeConfirmationWindow = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private Collectible collectible = null;
        [SerializeField] [Range(MinAmount, MaxAmount)] private int amount = MinAmount;

        #endregion

        #region PROPERTIES

        public int Amount { get => amount; }

        #endregion

        #region BEHAVIORS

        protected override bool IsAvailable()
        {
            return collectible != null;
        }

        protected override void Buy()
        {
            if (storeConfirmationWindow != null)
                LoadConfirmationWindow();
            else
                Spend();
        }

        private void LoadConfirmationWindow()
        {
            StoreConfirmationWindow confirmationWindow = CreateConfirmationWindow();
            confirmationWindow.Load(collectible.Sprite, FormatAmount(amount), storeReward.GetSprite(), FormatAmount(storeReward.GetQuantity()), Spend);
        }

        private StoreConfirmationWindow CreateConfirmationWindow()
        {
            return Instantiate(storeConfirmationWindow);
        }

        private string FormatAmount(int amount)
        {
            return string.Format(AmountFormat, amount);
        }

        private void Spend()
        {
            if (storeManager.TestMode)
            {
                storeReward.PurchaseCompleted();
                return;
            }

            bool spent = collectiblesManager.SpendCollectible(collectible, amount);

            if (spent)
                storeReward.PurchaseCompleted();
            else
                ShowNotAvailableWindow();
        }

        #endregion
    }
}
