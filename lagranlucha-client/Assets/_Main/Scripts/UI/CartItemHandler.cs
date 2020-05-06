using UnityEngine;

using TMPro;

using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public class CartItemHandler : ShopItemHandler
    {
        #region FIELDS

        [Header("PROPERTIES")]
        [SerializeField] private TextMeshProUGUI descriptionText;

        #endregion

        #region BEHAVIORS

        private void OnDestroy()
        {
            shoppingCartManager.onCartUpdated -= UpdateUI;
        }

        public void Initialize(Variant product)
        {
            shoppingCartManager.onCartUpdated += UpdateUI;
            this.product = product;
            SetupUI();
        }

        protected override void SetupUI()
        {
            nameText.text = product.Name;
            descriptionText.text = product.Description;
            priceText.text = PesosSign + product.Price.ToString();
            quantityText.text = quantity.ToString();
        }

        protected override void UpdateUI(int productId, int quantity)
        {
            if (productId != this.product.Id)
                return;

            quantityText.text = quantity.ToString();
        }

        #endregion
    }
}
