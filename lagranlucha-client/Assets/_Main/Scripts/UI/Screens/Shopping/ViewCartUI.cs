using UnityEngine;
using UnityEngine.UI;

using Zenject;

using LaGranLucha.API;
using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public class ViewCartUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private ShoppingCartManager shoppingCartManager;

        [Header("CART")]
        [SerializeField] private ViewCartItemHandler viewCartItemPrefab;
        [SerializeField] private Transform viewCartContainer;

        [Header("UI")]
        [SerializeField] private GameObject viewCart;
        [SerializeField] private Button editButton;
        [SerializeField] private Button viewCartButton;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            editButton.onClick.AddListener(EditCart);
            viewCartButton.onClick.AddListener(ViewCart);
        }

        public void ViewCart()
        {
            viewCart.SetActive(true);
            foreach (CartItem item in shoppingCartManager.Items)
                Instantiate<ViewCartItemHandler>(viewCartItemPrefab, viewCartContainer).Initialize(item);
        }

        private void EditCart()
        {
            viewCart.SetActive(false);
            RemoveProducts();
        }

        private void RemoveProducts()
        {
            foreach (Transform child in viewCartContainer)
                Destroy(child.gameObject);
        }

        #endregion
    }
}
