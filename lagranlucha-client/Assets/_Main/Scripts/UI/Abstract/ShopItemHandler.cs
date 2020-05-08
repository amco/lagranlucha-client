using UnityEngine;
using UnityEngine.UI;

using Zenject;
using TMPro;

using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public abstract class ShopItemHandler : ProductHandler
    {
        #region FIELDS

        protected const int MaxQuantity = 99;

        [Inject] protected ShoppingCartManager shoppingCartManager;

        [Header("HANDLER")]
        [SerializeField] protected TextMeshProUGUI quantityText;
        [SerializeField] protected Button addButton;
        [SerializeField] protected Button removeButton;
        
        protected int quantity = default(int);

        #endregion

        #region PROPERTIES

        protected string PesosSign { get => ShoppingCartManager.PesosSign; }

        #endregion

        #region BEHAVIORS

        protected abstract void UpdateUI(int productId, int quantity);

        private void Awake()
        {
            addButton.onClick.AddListener(AddItem);
            removeButton.onClick.AddListener(RemoveItem);
        }

        private void AddItem()
        {
            quantity++;
            quantity = Mathf.Clamp(quantity, default(int), MaxQuantity);
            shoppingCartManager.UpdateItem(product, quantity);
        }

        private void RemoveItem()
        {
            quantity--;
            quantity = Mathf.Clamp(quantity, default(int), MaxQuantity);
            shoppingCartManager.UpdateItem(product, quantity);
        }

        #endregion
    }
}
