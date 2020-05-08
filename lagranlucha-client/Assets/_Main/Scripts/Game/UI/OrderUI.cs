using System;
using UnityEngine;

using DG.Tweening;
using Zenject;

namespace LaGranLucha.Game
{
    public class OrderUI : MonoBehaviour
    {
        #region FIELDS

        private const float TrayMovementDuration = 0.5f;
        private const float Delay = 1f;
        private const int FirstChild = 0;
        private const int SecondChild = 1;
        private const int ThirdChild = 2;

        [Inject] private OrderManager orderManager = null;
        [Inject] private GameManager gameManager = null;

        [SerializeField] private Transform withHamburgerContainer = null;
        [SerializeField] private Transform noHamburgerContainer = null;
        [SerializeField] private Transform onlyOneContainer = null;
        [SerializeField] private GameIngredient gameIngredientPrefab = null;

        private Transform hamburgerContainer = null;
        private Transform friesContainer = null;
        private Transform drinkContainer = null;
        private RectTransform rectTransform = null;
        private Vector2 startPosition = Vector2.zero;

        #endregion

        #region EVENTS

        public event Action onTrayCleaned;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            startPosition = rectTransform.anchoredPosition;
            orderManager.onHamburgerIngredientSubmit += SendHamburgerIngredient;
            orderManager.onFriesIngredientSubmit += SendFriesIngredient;
            orderManager.onDrinkIngredientSubmit += SendDrinkIngredient;
            orderManager.onOrderCompleted += SendTrayToSide;
            orderManager.onHamburgerReset += Resethamburger;
            orderManager.onNewOrderStart += OrderStart;
            gameManager.onRoundStart += StartRound;
        }

        private void OnDestroy()
        {
            orderManager.onHamburgerIngredientSubmit -= SendHamburgerIngredient;
            orderManager.onFriesIngredientSubmit -= SendFriesIngredient;
            orderManager.onDrinkIngredientSubmit -= SendDrinkIngredient;
            orderManager.onOrderCompleted -= SendTrayToSide;
            orderManager.onHamburgerReset -= Resethamburger;
            orderManager.onNewOrderStart -= OrderStart;
            gameManager.onRoundStart -= StartRound;
        }

        private void StartRound(int round)
        {
            BringTrayToCenter();
        }

        private void OrderStart(Order order)
        {
            if (order.HaveHamburger && (order.HaveDrink || order.HaveFries))
            {
                friesContainer = withHamburgerContainer.GetChild(FirstChild);
                drinkContainer = withHamburgerContainer.GetChild(SecondChild);
                hamburgerContainer = withHamburgerContainer.GetChild(ThirdChild);
            }
            else if (order.HaveDrink && order.HaveFries)
            {
                friesContainer = noHamburgerContainer.GetChild(FirstChild);
                drinkContainer = noHamburgerContainer.GetChild(SecondChild);
                hamburgerContainer = null;
            }
            else
            {
                friesContainer = onlyOneContainer.GetChild(FirstChild);
                drinkContainer = onlyOneContainer.GetChild(SecondChild);
                hamburgerContainer = onlyOneContainer.GetChild(ThirdChild);
            }
        }

        private void SendTrayToSide()
        {
            rectTransform.DOAnchorPosX(Screen.width, TrayMovementDuration).SetDelay(Delay).SetEase(Ease.InBack).OnComplete(CleanOrder);
        }

        private void Resethamburger()
        {
            if (hamburgerContainer != null)
                foreach (Transform child in hamburgerContainer)
                    GameObject.Destroy(child.gameObject);
        }

        private void CleanOrder()
        {
            if (hamburgerContainer != null)
                foreach (Transform child in hamburgerContainer)
                    GameObject.Destroy(child.gameObject);

            if (friesContainer != null)
                foreach (Transform child in friesContainer)
                    GameObject.Destroy(child.gameObject);

            if (drinkContainer != null)
                foreach (Transform child in drinkContainer)
                    GameObject.Destroy(child.gameObject);

            onTrayCleaned?.Invoke();
            if (orderManager.HaveTimeRemaining)
                BringTrayToCenter();
        }

        private void BringTrayToCenter()
        {
            rectTransform.anchoredPosition = startPosition;
            rectTransform.DOAnchorPosX(-Screen.width, TrayMovementDuration).From().SetEase(Ease.OutBack);
        }

        private void SendHamburgerIngredient(Ingredient ingredient)
        {
            NewIngredient(ingredient, hamburgerContainer);
        }

        private void SendFriesIngredient(Ingredient ingredient)
        {
            NewIngredient(ingredient, friesContainer);
        }

        private void SendDrinkIngredient(Ingredient ingredient)
        {
            NewIngredient(ingredient, drinkContainer);
        }

        private void NewIngredient(Ingredient ingredient, Transform parent)
        {
            Instantiate(gameIngredientPrefab, parent).SetIngredient(ingredient);
        }

        #endregion
    }
}
