using System;
using UnityEngine;

namespace LaGranLucha.Game
{
    [Serializable]
    public class OrdersPerRound
    {
        #region FIELDS

        [SerializeField] private Order[] orders = null;

        #endregion

        #region PROPERTIES

        public Order RandomOrder { get => orders[UnityEngine.Random.Range(0, orders.Length)]; }

        #endregion
    }
}
