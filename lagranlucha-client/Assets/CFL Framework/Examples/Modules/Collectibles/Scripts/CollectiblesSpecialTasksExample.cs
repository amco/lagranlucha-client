using UnityEngine;

using Zenject;

namespace CFLFramework.Collectibles
{
    public class CollectiblesSpecialTasksExample : MonoBehaviour
    {
        #region FIELDS

        private const int CopperNeededForSilver = 100;
        private const int SilverNeededForGold = 100;

        private const string CoinsTag = "coins";

        private const string CopperCoin = "copper";
        private const string SilverCoin = "silver";
        private const string GoldCoin = "gold";

        [Inject] private CollectiblesManager collectiblesManager = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            collectiblesManager.onCollectibleIncreased += CustomOnCollectibleIncreased;
        }

        private void OnDestroy()
        {
            collectiblesManager.onCollectibleIncreased -= CustomOnCollectibleIncreased;
        }

        private void CustomOnCollectibleIncreased(Collectible collectible, int amount)
        {
            switch (collectible.name)
            {
                case CopperCoin:
                    CreateSilverCoins(collectible);
                    break;
                case SilverCoin:
                    CreateGoldCoins(collectible);
                    break;
            }
        }

        private void CreateSilverCoins(Collectible copper)
        {
            if (copper.Quantity < CopperNeededForSilver)
                return;

            float silverCoins = Mathf.Floor(copper.Quantity / CopperNeededForSilver);
            Collectible silver = collectiblesManager.GetCollectible(CoinsTag, SilverCoin);

            copper.SpendCollectible(silverCoins * CopperNeededForSilver);
            silver.IncreaseCollectible(silverCoins);
            CreateGoldCoins(silver);
        }

        private void CreateGoldCoins(Collectible silver)
        {
            if (silver.Quantity < SilverNeededForGold)
                return;

            float goldCoins = Mathf.Floor(silver.Quantity / SilverNeededForGold);
            Collectible gold = collectiblesManager.GetCollectible(CoinsTag, GoldCoin);

            silver.SpendCollectible(goldCoins * SilverNeededForGold);
            gold.IncreaseCollectible(goldCoins);
        }

        #endregion
    }
}
