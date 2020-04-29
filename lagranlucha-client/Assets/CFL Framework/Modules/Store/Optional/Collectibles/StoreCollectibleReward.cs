using UnityEngine;

using Zenject;
using CFLFramework.Collectibles;

namespace CFLFramework.Store
{
    public class StoreCollectibleReward : StoreReward
    {
        #region FIELDS

        private const int MinAmount = 0;
        private const int MaxAmount = 9999;

        [Inject] private CollectiblesManager collectiblesManager = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private Collectible collectible = null;
        [SerializeField] [Range(MinAmount, MaxAmount)] private int amount = MinAmount;

        #endregion

        #region BEHAVIORS

        public override void GiveReward()
        {
            collectiblesManager.IncreaseCollectible(collectible, amount);
        }

        public override Sprite GetSprite()
        {
            return collectible.Sprite;
        }

        public override int GetQuantity()
        {
            return amount;
        }

        #endregion
    }
}
