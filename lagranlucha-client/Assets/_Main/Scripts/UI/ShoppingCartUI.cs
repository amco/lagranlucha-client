using UnityEngine;

using Zenject;
using TMPro;

using LaGranLucha.API;
using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public class ShoppingCartUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private ShoppingCartManager shoppingCartManager;

        [SerializeField] private ViewCartItemHandler viewCartItemPrefab;
        [SerializeField] private Transform viewCartContainer;
        [SerializeField] private TextMeshProUGUI totalText;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            shoppingCartManager.onCartUpdated += OnCartUpdated;
        }

        private void OnDestroy()
        {
            shoppingCartManager.onCartUpdated -= OnCartUpdated;
        }

        private void OnCartUpdated(int productId, int quantity)
        {
            totalText.text = ShoppingCartManager.PesosSign + shoppingCartManager.GetTotal().ToString();
        }

        private void ViewCart()
        {
            foreach (CartItem item in shoppingCartManager.Items)
                Instantiate<ViewCartItemHandler>(viewCartItemPrefab, viewCartContainer).Initialize(item);
        }

        #endregion
    }
}
