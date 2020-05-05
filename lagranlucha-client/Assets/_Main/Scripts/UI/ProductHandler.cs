using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Zenject;

using LaGranLucha.API;
using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public class ProductHandler : MonoBehaviour
    {
        #region FIELDS

        private const string PesosSign = "$";
        private const int MaxQuantity = 99;

        [Inject] private ShoppingCartManager shoppingCart;

        [Header("PROPERTIES")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Image previewImage;

        [Header("HANDLER")]
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private Button addButton;
        [SerializeField] private Button removeButton;

        private Variant product;
        private int quantity = default(int);

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            addButton.onClick.AddListener(AddItem);
            removeButton.onClick.AddListener(RemoveItem);
        }

        public void Initialize(Variant product)
        {
            this.product = product;
            SetupUI();
        }

        private void SetupUI()
        {
            nameText.text = product.Name;
            descriptionText.text = product.Description;
            priceText.text = PesosSign + product.Price.ToString();
            quantityText.text = quantity.ToString();
        }

        private void AddItem()
        {
            quantity++;
            quantity = Mathf.Clamp(quantity, default(int), MaxQuantity);
            UpdateItem();
        }

        private void RemoveItem()
        {
            quantity--;
            quantity = Mathf.Clamp(quantity, default(int), MaxQuantity);
            UpdateItem();
        }

        private void UpdateItem()
        {
            quantityText.text = quantity.ToString();
            shoppingCart.UpdateItem(product, quantity);
        }

        #endregion
    }
}
