using System;
using UnityEngine;

namespace CFLFramework.Store
{
    public abstract class StoreReward : MonoBehaviour
    {
        #region FIELDS

        protected const int DefaultQuantity = 1;

        #endregion

        #region EVENTS

        public event Action onPurchaseCompleted;

        #endregion

        #region BEHAVIORS

        public abstract Sprite GetSprite();
        public abstract int GetQuantity();
        public abstract void GiveReward();

        public void PurchaseCompleted()
        {
            GiveReward();
            onPurchaseCompleted?.Invoke();
        }

        #endregion
    }
}
