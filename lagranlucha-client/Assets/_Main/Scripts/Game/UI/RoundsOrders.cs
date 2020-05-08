using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LaGranLucha.Game
{
    public class RoundsOrders : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private List<OrdersPerRound> ordersPerRound = null;

        #endregion

        #region BEHAVIORS

        public Order NewOrder(int round)
        {
            if (round < ordersPerRound.Count)
                return ordersPerRound[round].RandomOrder;
            else
                return ordersPerRound.Last().RandomOrder;
        }

        #endregion
    }
}
