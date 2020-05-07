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
        [SerializeField] private List<ShownIngredient> hamburguerIngredients = null;
        [SerializeField] private ShownIngredient friesIngredient = null;
        [SerializeField] private ShownIngredient drinkIngredient = null;
        [SerializeField] private TMP_Text shownName = null;
        [SerializeField] private NumberCounter timer = null;
        [SerializeField] private NumberCounter stars = null;
        [SerializeField] private GameObject starParticlePrefab = null;
        [SerializeField] private GameObject currentIngredientArrow = null;
        [SerializeField] private OrderBar orderBar = null;

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
        public bool HaveTimeRemaining { get => timer.IntValue > default(int); }

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
            Invoke(nameof(NewOrder), DelayRoundStart);
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
                friesIngredient.Hide();
                completedFries = true;
            }
            else if (ingredient == GetDrinkIngredient())
            {
                onDrinkIngredientSubmit(ingredient);
                drinkIngredient.Hide();
                completedDrink = true;
            }
            else if (ingredient == GetHamburguerIngredient(hamburguerStep))
            {
                onHamburguerIngredientSubmit(ingredient);
                hamburguerIngredients[hamburguerStep].Hide();
                hamburguerStep++;
                if (hamburguerStep >= order.HamburgerIngredientsCount)
                    completedHamburguer = true;
                else
                    hamburguerIngredients[hamburguerStep].SetAsCurrentIngredient();
            }
            else
            {
                if (ingredient.IsHamburguerIngredient && !completedHamburguer)
                    ResetBurguer();

                ShowFail();
            }

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
            for (int i = 0; i < order.HamburgerIngredientsCount; i++)
            {
                hamburguerIngredients[i].Show();
                hamburguerIngredients[i].SetAsQueuedIngredient();
            }

            hamburguerIngredients.First().SetAsCurrentIngredient();
        }

        private void CompletedOrder()
        {
            onOrderCompleted?.Invoke();
            shownName.text = string.Empty;
            currentIngredientArrow.gameObject.SetActive(false);
            Invoke(nameof(SpawnParticles), ParticlesSpawnDelay);
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
            for (int i = 0; i < hamburguerIngredients.Count; i++)
            {
                Ingredient ingredient = GetHamburguerIngredient(i);
                hamburguerIngredients[i].SetIngredient(ingredient);
                hamburguerIngredients[i].SetAsQueuedIngredient();
            }

            hamburguerIngredients.First().SetAsCurrentIngredient();
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
