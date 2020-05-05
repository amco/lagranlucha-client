using System.Collections.Generic;
using UnityEngine;

using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public class OrderManager : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private int taxRate = 16;

        #endregion

        #region PROPERTIES

        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        #endregion

        #region BEHAVIORS

        public void UpdateItem(Variant product, int quantity)
        {
            CartItem newItem = new CartItem(product);
            if (Items.Contains(newItem))
            {
                foreach (CartItem item in Items)
                {
                    if (item.Equals(newItem))
                    {
                        if (quantity <= default(int))
                        {
                            RemoveItem(item.Product);
                            return;
                        }

                        item.Quantity = quantity;
                        return;
                    }
                }
            }
            else
            {
                newItem.Quantity = quantity;
                Items.Add(newItem);
            }
        }

        public float GetSubtotal()
        {
            float subTotal = default(float);
            foreach (CartItem item in Items)
                subTotal += item.TotalPrice;

            return subTotal;
        }

        public float GetTotal()
        {
            float total = default(float);
            float subTotal = GetSubtotal();
            float taxAmount = subTotal * (taxRate / 100f);
            total = subTotal + taxAmount;
            return total;
        }

        private void RemoveItem(Variant product)
        {
            CartItem removedItem = new CartItem(product);
            Items.Remove(removedItem);
        }

        #endregion
    }
}
