using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Zenject;

namespace LaGranLucha.Game
{
    public class OrderManager : MonoBehaviour
    {
        #region FIELDS

        private const float ParticlesSpawnDelay = 1.25f;
        private const float DelayRoundStart = 1.5f;
        private const float DelayForEndRound = 1.5f;

        [Inject] private GameManager gameManager = null;
        [Inject] private ParticlesManager particlesManager = null;
        [Inject] private OrderUI orderUI = null;

        [SerializeField] private RoundsOrders roundOrder = null;
        [SerializeField] private List<ShownIngredient> hamburgerIngredients = null;
        [SerializeField] private ShownIngredient friesIngredient = null;
        [SerializeField] private ShownIngredient drinkIngredient = null;
        [SerializeField] private TMP_Text shownName = null;
        [SerializeField] private NumberCounter timer = null;
        [SerializeField] private NumberCounter stars = null;
        [SerializeField] private GameObject starParticlePrefab = null;
        [SerializeField] private GameObject currentIngredientArrow = null;
        [SerializeField] private OrderBar orderBar = null;

        private int hamburgerStep = 0;
        private bool completedHamburger = false;
        private bool completedFries = false;
        private bool completedDrink = false;
        private Order order = null;

        #endregion

        #region EVENTS

        public event Action<Ingredient> onHamburgerIngredientSubmit;
        public event Action<Ingredient> onFriesIngredientSubmit;
        public event Action<Ingredient> onDrinkIngredientSubmit;
        public event Action onHamburgerReset;
        public event Action onOrderCompleted;
        public event Action<Order> onNewOrderStart;

        #endregion

        #region PROPERTIES

        public bool OrderComplete { get => completedHamburger && completedFries && completedDrink; }
        public bool HaveTimeRemaining { get => timer.IntValue > default(int); }
        public float ThisRoundHamburgersScore { get; private set; }
        public float ThisRoundFriesScore { get; private set; }
        public float ThisRoundDrinksScore { get; private set; }
        public float ThisRoundTotalScore { get => ThisRoundHamburgersScore + ThisRoundFriesScore + ThisRoundDrinksScore; }

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            orderUI.onTrayCleaned += OrderSent;
            gameManager.onRoundStart += StartRound;
        }

        private void OnDestroy()
        {
            orderUI.onTrayCleaned -= OrderSent;
            gameManager.onRoundStart -= StartRound;
        }

        private void StartRound(int round)
        {
            ThisRoundHamburgersScore = ThisRoundFriesScore = ThisRoundDrinksScore = default(int);
            Invoke(nameof(NewOrder), DelayRoundStart);
        }

        private void NewOrder()
        {
            order = roundOrder.NewOrder(gameManager.CurrentRound);
            order.GetRandomElements();
            currentIngredientArrow.SetActive(order.HaveHamburger);
            completedHamburger = !order.HaveHamburger;
            completedFries = !order.HaveFries;
            completedDrink = !order.HaveDrink;
            hamburgerStep = default(int);
            SetGraphics();
            shownName.text = order.OrderName;
            onNewOrderStart?.Invoke(order);
        }

        public void SendIngredient(Ingredient ingredient)
        {
            if (ingredient == GetFriesIngredient())
            {
                onFriesIngredientSubmit(ingredient);
                friesIngredient.Hide();
                completedFries = true;
            }
            else if (ingredient == GetDrinkIngredient())
            {
                onDrinkIngredientSubmit(ingredient);
                drinkIngredient.Hide();
                completedDrink = true;
            }
            else if (ingredient == GetHamburgerIngredient(hamburgerStep))
            {
                onHamburgerIngredientSubmit(ingredient);
                hamburgerIngredients[hamburgerStep].Hide();
                hamburgerStep++;
                if (hamburgerStep >= order.HamburgerIngredientsCount)
                    completedHamburger = true;
                else
                    hamburgerIngredients[hamburgerStep].SetAsCurrentIngredient();
            }
            else
            {
                if (ingredient.IsHamburgerIngredient && !completedHamburger)
                    ResetBurguer();

                ShowFail();
            }

            if (OrderComplete)
                CompletedOrder();
        }

        private void ShowFail()
        {
            //throw new NotImplementedException();
        }

        private void ResetBurguer()
        {
            hamburgerStep = default(int);
            onHamburgerReset?.Invoke();
            for (int i = 0; i < order.HamburgerIngredientsCount; i++)
            {
                hamburgerIngredients[i].Show();
                hamburgerIngredients[i].SetAsQueuedIngredient();
            }

            hamburgerIngredients.First().SetAsCurrentIngredient();
        }

        private void CompletedOrder()
        {
            onOrderCompleted?.Invoke();
            shownName.text = string.Empty;
            currentIngredientArrow.gameObject.SetActive(false);
            Invoke(nameof(SpawnParticles), ParticlesSpawnDelay);
            if (orderBar.OnTime)
            {
                ThisRoundHamburgersScore += order.HamburgerScore;
                ThisRoundFriesScore += order.FriesScore;
                ThisRoundDrinksScore += order.DrinkScore;
            }
            else
            {
                ThisRoundHamburgersScore += order.DelayedHamburgerScore;
                ThisRoundFriesScore += order.DelayedFriesScore;
                ThisRoundDrinksScore += order.DelayedDrinkScore;
            }
        }

        private void SpawnParticles()
        {
            particlesManager.SpawnParticles(starParticlePrefab, stars, Vector2.zero, orderBar.OnTime ? order.OrderScore : order.DelayedOrderScore);
        }

        private void OrderSent()
        {
            if (HaveTimeRemaining)
                NewOrder();
            else
                Invoke(nameof(EndRound), DelayForEndRound);
        }

        private void EndRound()
        {
            gameManager.EndRound();
        }

        private void SetGraphics()
        {
            for (int i = 0; i < hamburgerIngredients.Count; i++)
            {
                Ingredient ingredient = GetHamburgerIngredient(i);
                hamburgerIngredients[i].SetIngredient(ingredient);
                hamburgerIngredients[i].SetAsQueuedIngredient();
            }

            hamburgerIngredients.First().SetAsCurrentIngredient();
            friesIngredient.SetIngredient(GetFriesIngredient());
            drinkIngredient.SetIngredient(GetDrinkIngredient());
        }

        private Ingredient GetHamburgerIngredient(int step)
        {
            if (!order.HaveHamburger || step >= order.Hamburger.Ingredients.Count)
                return null;

            return order.Hamburger.Ingredients[step];
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
