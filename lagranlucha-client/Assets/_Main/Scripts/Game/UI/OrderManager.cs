using System;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace LaGranLucha.Game
{
    public class OrderManager : MonoBehaviour
    {
        #region FIELDS

        private const string OneProductFormat = "{0}";
        private const string TwoProductsFormat = "{0} y {1}";
        private const string ThreeProductsFormat = "Combo";

        private static readonly string[] ProductNameFormats = new string[] { string.Empty, OneProductFormat, TwoProductsFormat, ThreeProductsFormat };

        [SerializeField] private Order[] possibleOrders = null;
        [SerializeField] private ShownIngredient[] shownIngredients = null;
        [SerializeField] private TMP_Text shownName = null;
        [SerializeField] private NumberCounter coins = null;
        [SerializeField] private NumberCounter discount = null;


        private Product hamburguer = null;
        private Product fries = null;
        private Product drink = null;
        private int step = 0;

        #endregion

        #region EVENTS

        public event Action<Ingredient> onHamburguerIngredientSubmit;
        public event Action<Ingredient> onFriesIngredientSubmit;
        public event Action<Ingredient> onDrinkIngredientSubmit;
        public event Action onOrderCompleted;
        public event Action onNewOrderStart;

        #endregion

        #region PROPERTIES

        public bool OrderComplete { get => step >= HamburgerIngredientsCount + FriesIngredientsCount + DrinkIngredientsCount; }
        public bool HaveHamburger { get => hamburguer != null; }
        public bool HaveFries { get => fries != null; }
        public bool HaveDrink { get => drink != null; }
        public int HamburgerIngredientsCount { get => hamburguer != null ? hamburguer.Ingredients.Count : 0; }
        public int FriesIngredientsCount { get => fries != null ? fries.Ingredients.Count : 0; }
        public int DrinkIngredientsCount { get => drink != null ? drink.Ingredients.Count : 0; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            NewOrder();
        }

        private void NewOrder()
        {
            Order newOrder = possibleOrders[UnityEngine.Random.Range(0, possibleOrders.Length)];
            hamburguer = newOrder.Hamburguer.Count > 0 ? newOrder.Hamburguer[UnityEngine.Random.Range(0, newOrder.Hamburguer.Count)] : null;
            fries = newOrder.Fries.Count > 0 ? newOrder.Fries[UnityEngine.Random.Range(0, newOrder.Fries.Count)] : null;
            drink = newOrder.Drink.Count > 0 ? newOrder.Drink[UnityEngine.Random.Range(0, newOrder.Drink.Count)] : null;
            step = 0;
            SetGraphics();
            SetName();
            onNewOrderStart?.Invoke();
        }

        private void SetName()
        {
            List<string> names = new List<string>();
            if (HaveHamburger)
                names.Add(hamburguer.Name);

            if (HaveFries)
                names.Add(fries.Name);

            if (HaveDrink)
                names.Add(drink.Name);

            shownName.text = string.Format(ProductNameFormats[names.Count], names.ToArray());
        }

        public void SendIngredient(Ingredient ingredient)
        {
            if (ingredient == GetIngredient(step))
            {
                if (step < HamburgerIngredientsCount)
                    onHamburguerIngredientSubmit(ingredient);
                else if (step < HamburgerIngredientsCount + FriesIngredientsCount)
                    onFriesIngredientSubmit(ingredient);
                else if (step < HamburgerIngredientsCount + FriesIngredientsCount + DrinkIngredientsCount)
                    onDrinkIngredientSubmit(ingredient);

                NextStep();
            }
        }

        private void NextStep()
        {
            step++;
            SetGraphics();
            if (OrderComplete)
            {
                onOrderCompleted?.Invoke();
                int coinsIncrease = 0;
                float discountIncrease = 0;

                if (HaveHamburger)
                {
                    coinsIncrease += hamburguer.Coins;
                    discountIncrease += hamburguer.Discount;
                }

                if (HaveFries)
                {
                    coinsIncrease += fries.Coins;
                    discountIncrease += fries.Discount;
                }

                if (HaveDrink)
                {
                    coinsIncrease += drink.Coins;
                    discountIncrease += drink.Discount;
                }

                coins.Increase(coinsIncrease);
                discount.Increase(discountIncrease);
                Invoke(nameof(NewOrder), 1f);
            }
        }

        private void SetGraphics()
        {
            for (int i = 0; i < shownIngredients.Length; i++)
            {
                Ingredient ingredient = GetIngredient(step + i);
                shownIngredients[i].SetIngredient(ingredient);
            }
        }

        private Ingredient GetIngredient(int step)
        {
            if (HamburgerIngredientsCount > step)
                return hamburguer.Ingredients[step];

            step -= HamburgerIngredientsCount;
            if (FriesIngredientsCount > step)
                return fries.Ingredients[step];

            step -= FriesIngredientsCount;
            if (DrinkIngredientsCount > step)
                return drink.Ingredients[step];

            return null;
        }

        #endregion
    }
}
