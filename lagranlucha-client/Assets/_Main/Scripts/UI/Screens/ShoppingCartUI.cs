using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Zenject;
using CFLFramework.API;

using LaGranLucha.API;
using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public class ShoppingCartUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LaGranLuchaManager laGranLuchaManager;
        [Inject] private ShoppingCartManager shoppingCartManager;

        [Header("CART")]
        [SerializeField] private CartItemHandler cartItemHandler;
        [SerializeField] private Transform foodContainer;
        [SerializeField] private Transform drinkContainer;

        [Header("UI")]
        [SerializeField] private ViewCartUI viewCartUI;
        [SerializeField] private Button finishOrderButton;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI totalText;
        [SerializeField] private GameObject cartNotification;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            backButton.onClick.AddListener(GoBackBranches);
            shoppingCartManager.onCartUpdated += OnCartUpdated;
        }

        private void OnEnable()
        {
            laGranLuchaManager.GetFoods(OnGetFoods);
            laGranLuchaManager.GetDrinks(OnGetDrinks);
        }

        private void Start()
        {
            finishOrderButton.onClick.AddListener(viewCartUI.ViewCart);
        }

        private void OnDestroy()
        {
            shoppingCartManager.onCartUpdated -= OnCartUpdated;
        }

        private void OnGetFoods(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            foreach (Food food in laGranLuchaManager.Foods)
                foreach (Variant variant in food.Variants)
                    Instantiate<CartItemHandler>(cartItemHandler, foodContainer).Initialize(variant);
        }

        private void OnGetDrinks(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            foreach (Drink drink in laGranLuchaManager.Drinks)
                foreach (Variant variant in drink.Variants)
                    Instantiate<CartItemHandler>(cartItemHandler, drinkContainer).Initialize(variant);
        }

        private void OnCartUpdated(int productId, int quantity)
        {
            totalText.text = ShoppingCartManager.PesosSign + shoppingCartManager.GetTotal().ToString();
            if (shoppingCartManager.Items.Count <= default(int))
            {
                cartNotification.SetActive(false);
                finishOrderButton.gameObject.SetActive(false);
                return;
            }

            cartNotification.SetActive(true);
            finishOrderButton.gameObject.SetActive(true);
            cartNotification.GetComponentInChildren<TextMeshProUGUI>().text = shoppingCartManager.Items.Count.ToString();
        }

        private void GoBackBranches()
        {
            cartNotification.SetActive(false);
            finishOrderButton.gameObject.SetActive(false);
            laGranLuchaManager.OpenBranchScene();
            RemoveProducts(foodContainer);
            RemoveProducts(drinkContainer);
            shoppingCartManager.CleanShoppingCart();
            gameObject.SetActive(false);
        }

        private void RemoveProducts(Transform container)
        {
            foreach (Transform child in container)
                Destroy(child.gameObject);
        }

        #endregion
    }
}
