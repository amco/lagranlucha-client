using UnityEngine;

using Zenject;
using CFLFramework.Lives;

namespace CFLFramework.Store
{
    public class StoreLivesReward : StoreReward
    {
        #region FIELDS

        private const int MinAmount = 1;
        private const int MaxAmount = 5;

        [Inject] private LivesManager livesManager = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] [Range(MinAmount, MaxAmount)] private int lives = MinAmount;

        #endregion

        #region BEHAVIORS

        public override void GiveReward()
        {
            livesManager.AddLives(lives);
        }

        public override Sprite GetSprite()
        {
            return livesManager.LifeSprite;
        }

        public override int GetQuantity()
        {
            return lives;
        }

        #endregion
    }
}
