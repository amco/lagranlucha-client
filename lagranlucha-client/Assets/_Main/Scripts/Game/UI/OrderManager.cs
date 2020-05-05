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
        [SerializeField] private ShownIngredient[] hamburguerIngredients = null;
        [SerializeField] private ShownIngredient friesIngredient = null;
        [SerializeField] private ShownIngredient drinkIngredient = null;
        [SerializeField] private TMP_Text shownName = null;
        [SerializeField] private OrderBar orderBar = null;
        [SerializeField] private NumberCounter coins = null;
        [SerializeField] private NumberCounter discount = null;

        private Product hamburguer = null;
        private Product fries = null;
        private Product drink = null;
        private int hamburguerStep = 0;
        private bool completedHamburguer = false;
        private bool completedFries = false;
        private bool completedDrink = false;

        #endregion

        #region EVENTS

        public event Action<Ingredient> onHamburguerIngredientSubmit;
        public event Action<Ingredient> onFriesIngredientSubmit;
        public event Action<Ingredient> onDrinkIngredientSubmit;
        public event Action onHamburguerReset;
        public event Action onOrderCompleted;
        public event Action onNewOrderStart;

        #endregion

        #region PROPERTIES

        public bool OrderComplete { get => completedHamburguer && completedFries && completedDrink; }
        public bool HaveHamburger { get => hamburguer != null; }
        public bool HaveFries { get => fries != null; }
        public bool HaveDrink { get => drink != null; }
        public int HamburgerIngredientsCount { get => hamburguer != null ? hamburguer.Ingredients.Count : 0; }
        public int FriesIngredientsCount { get => fries != null ? fries.Ingredients.Count : 0; }
        public int DrinkIngredientsCount { get => drink != null ? drink.Ingredients.Count : 0; }

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            NewOrder();
        }

        private void NewOrder()
        {
            Order newOrder = possibleOrders[UnityEngine.Random.Range(0, possibleOrders.Length)];
            hamburguer = newOrder.Hamburguer.Count > 0 ? newOrder.Hamburguer[UnityEngine.Random.Range(0, newOrder.Hamburguer.Count)] : null;
            fries = newOrder.Fries.Count > 0 ? newOrder.Fries[UnityEngine.Random.Range(0, newOrder.Fries.Count)] : null;
            drink = newOrder.Drink.Count > 0 ? newOrder.Drink[UnityEngine.Random.Range(0, newOrder.Drink.Count)] : null;
            completedHamburguer = !HaveHamburger;
            completedFries = !HaveFries;
            completedDrink = !HaveDrink;
            hamburguerStep = 0;
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
                if (hamburguerStep >= HamburgerIngredientsCount)
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
            if (!HaveHamburger || step >= hamburguer.Ingredients.Count)
                return null;
            return hamburguer.Ingredients[step];
        }

        private Ingredient GetFriesIngredient()
        {
            if (!HaveFries || completedFries)
                return null;

            return fries.Ingredients[default(int)];
        }

        private Ingredient GetDrinkIngredient()
        {
            if (!HaveDrink || completedDrink)
                return null;

            return drink.Ingredients[default(int)];
        }

        #endregion
    }
}
