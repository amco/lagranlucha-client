using System;
using UnityEngine;

using TMPro;
using Zenject;

namespace LaGranLucha.Game
{
    public class OrderManager : MonoBehaviour
    {
        #region FIELDS

        private const float DelayForNewOrder = 1.5f;
        private const float DelayForEndRound = 2f;

        [Inject] private GameManager gameManager = null;

        [SerializeField] private RoundsOrders roundOrder = null;
        [SerializeField] private ShownIngredient[] hamburguerIngredients = null;
        [SerializeField] private ShownIngredient friesIngredient = null;
        [SerializeField] private ShownIngredient drinkIngredient = null;
        [SerializeField] private TMP_Text shownName = null;
        [SerializeField] private NumberCounter timer = null;
        [SerializeField] private NumberCounter discount = null;
        [SerializeField] private GameObject currentIngredientArrow = null;

        private int hamburguerStep = 0;
        private bool completedHamburguer = false;
        private bool completedFries = false;
        private bool completedDrink = false;
        private Order order = null;

        #endregion

        #region EVENTS

        public event Action<Ingredient> onHamburguerIngredientSubmit;
        public event Action<Ingredient> onFriesIngredientSubmit;
        public event Action<Ingredient> onDrinkIngredientSubmit;
        public event Action onHamburguerReset;
        public event Action onOrderCompleted;
        public event Action<Order> onNewOrderStart;

        #endregion

        #region PROPERTIES

        public bool OrderComplete { get => completedHamburguer && completedFries && completedDrink; }
        public int TimeRemaining { get => timer.IntValue; }

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            gameManager.onRoundStart += StartRound;
        }

        private void StartRound(int round)
        {
            Invoke(nameof(NewOrder), DelayForNewOrder);
        }

        private void NewOrder()
        {
            order = roundOrder.NewOrder(gameManager.CurrentRound);
            order.GetRandomElements();
            currentIngredientArrow.SetActive(order.HaveHamburger);
            completedHamburguer = !order.HaveHamburger;
            completedFries = !order.HaveFries;
            completedDrink = !order.HaveDrink;
            hamburguerStep = 0;
            SetGraphics();
            shownName.text = order.OrderName;
            onNewOrderStart?.Invoke(order);
        }

        public void SendIngredient(Ingredient ingredient)
        {
            if (ingredient == GetFriesIngredient())
            {
                onFriesIngredientSubmit(ingredient);
                completedFries = true;
            }
            else if (ingredient == GetDrinkIngredient())
            {
                onDrinkIngredientSubmit(ingredient);
                completedDrink = true;
            }
            else if (ingredient == GetHamburguerIngredient(hamburguerStep))
            {
                onHamburguerIngredientSubmit(ingredient);
                hamburguerStep++;
                if (hamburguerStep >= order.HamburgerIngredientsCount)
                    completedHamburguer = true;
            }
            else
            {
                if (ingredient.IsHamburguerIngredient && !completedHamburguer)
                    ResetBurguer();

                ShowFail();
            }

            SetGraphics();
            if (OrderComplete)
                CompletedOrder();
        }

        private void ShowFail()
        {
            Debug.Log("Error");
        }

        private void ResetBurguer()
        {
            hamburguerStep = 0;
            onHamburguerReset?.Invoke();
        }

        private void CompletedOrder()
        {
            onOrderCompleted?.Invoke();
            shownName.text = string.Empty;
            currentIngredientArrow.gameObject.SetActive(false);
            discount.Increase(order.OrderScore);
            if (TimeRemaining > 0)
                Invoke(nameof(NewOrder), DelayForNewOrder);
            else
                Invoke(nameof(EndRound), DelayForEndRound);
        }

        private void EndRound()
        {
            gameManager.EndRound();
        }

        private void SetGraphics()
        {
            for (int i = 0; i < hamburguerIngredients.Length; i++)
            {
                Ingredient ingredient = GetHamburguerIngredient(hamburguerStep + i);
                hamburguerIngredients[i].SetIngredient(ingredient);
            }

            friesIngredient.SetIngredient(GetFriesIngredient());
            drinkIngredient.SetIngredient(GetDrinkIngredient());
        }

        private Ingredient GetHamburguerIngredient(int step)
        {
            if (!order.HaveHamburger || step >= order.Hamburguer.Ingredients.Count)
                return null;
            return order.Hamburguer.Ingredients[step];
        }

        private Ingredient GetFriesIngredient()
        {
            if (!order.HaveFries || completedFries)
                return null;

            return order.Fries.Ingredients[default(int)];
        }

        private Ingredient GetDrinkIngredient()
        {
            if (!order.HaveDrink || completedDrink)
                return null;

            return order.Drink.Ingredients[default(int)];
        }

        #endregion
    }
}
