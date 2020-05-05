using System.Collections.Generic;
using UnityEngine;

using Zenject;
using CFLFramework.API;

using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public class ShoppingCartManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] private BranchManager branchManager;
        [Inject] private APIManager apiManager;

        #endregion

        #region PROPERTIES

        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        #endregion

        #region BEHAVIORS

        public void CreateOrder()
        {
            Order order = new Order();
            foreach (CartItem item in Items)
            {
                OrderProduct orderProduct = new OrderProduct();
                orderProduct.VariantId = item.VariantId;
                orderProduct.Quantity = item.Quantity;
                order.LineItems.Add(orderProduct);
            }

            order.BranchId = branchManager.CurrentBranch.Id;
            apiManager.SendFakeRequestSucceded(order, null);
        }

        public Variant GetProductFromCart(Variant product)
        {
            CartItem newItem = new CartItem(product);
            if (Items.Contains(newItem))
            {
                foreach (CartItem item in Items)
                {
                    if (item.Equals(newItem))
                        return item.Product;
                }
            }

            return null;
        }

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

        public float GetTotal()
        {
            float subTotal = default(float);
            foreach (CartItem item in Items)
                subTotal += item.TotalPrice;

            return subTotal;
        }

        private void RemoveItem(Variant product)
        {
            CartItem removedItem = new CartItem(product);
            Items.Remove(removedItem);
        }

        #endregion
    }
}
