using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public class ViewCartItemHandler : ShopItemHandler
    {
        #region FIELDS

        private CartItem cartItem;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            shoppingCartManager.onCartUpdated += UpdateUI;
        }

        private void OnDestroy()
        {
            shoppingCartManager.onCartUpdated -= UpdateUI;
        }

        public void Initialize(CartItem cartItem)
        {
            this.cartItem = cartItem;
            product = cartItem.Product;
            SetupUI();
        }

        protected override void SetupUI()
        {
            nameText.text = product.Name;
            quantityText.text = (quantity = cartItem.Quantity).ToString();
            priceText.text = PesosSign + cartItem.TotalPrice.ToString();
        }

        protected override void UpdateUI(int productId, int quantity)
        {
            if (productId != this.product.Id)
                return;

            if (quantity <= default(int))
                Destroy(gameObject);

            quantityText.text = quantity.ToString();
            priceText.text = PesosSign + cartItem.TotalPrice.ToString();
        }

        #endregion
    }
}
