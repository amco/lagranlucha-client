using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Zenject;
using CFLFramework.API;

using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public class ShoppingCartManager : MonoBehaviour
    {
        #region FIELDS

        public const string PesosSign = "$";

        [Inject] private BranchManager branchManager;
        [Inject] private APIManager apiManager;

        #endregion

        #region EVENTS

        public event UnityAction<int, int> onCartUpdated;

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
                            item.Quantity = default(int);
                            onCartUpdated?.Invoke(item.Product.Id, item.Quantity);
                            RemoveItem(item.Product);
                            break;
                        }

                        item.Quantity = quantity;
                        onCartUpdated?.Invoke(item.Product.Id, item.Quantity);
                        break;
                    }
                }
            }
            else
            {
                newItem.Quantity = quantity;
                Items.Add(newItem);
                onCartUpdated?.Invoke(newItem.Product.Id, newItem.Quantity);
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
