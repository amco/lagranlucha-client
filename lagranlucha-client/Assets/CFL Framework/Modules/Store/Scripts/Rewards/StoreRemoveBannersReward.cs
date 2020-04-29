using UnityEngine;

using Zenject;

namespace CFLFramework.Store
{
    public class StoreRemoveBannersReward : StoreReward
    {
        #region FIELDS

        [Inject] private StoreManager storeManager = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private Sprite rewardSprite = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            storeManager.onBannersRemoved += AppearElement;
        }

        private void Start()
        {
            AppearElement(storeManager.RemovedAds);
        }

        public override void GiveReward()
        {
            storeManager.RemoveAds();
        }

        public override Sprite GetSprite()
        {
            return rewardSprite;
        }

        public override int GetQuantity()
        {
            return DefaultQuantity;
        }

        private void AppearElement(bool removed)
        {
            gameObject.SetActive(!removed);
        }

        #endregion
    }
}
