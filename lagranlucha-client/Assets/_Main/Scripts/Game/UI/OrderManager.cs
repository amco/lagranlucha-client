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
        private const string ThreeProductsFormat = "{0} con {1} y {2}";

        private static readonly string[] ProductNameFormats = new string[] { string.Empty, OneProductFormat, TwoProductsFormat, ThreeProductsFormat };

        [SerializeField] private Order[] possibleOrders = null;
        [SerializeField] private ShownIngredient[] shownIngredients = null;
        [SerializeField] private TMP_Text shownName = null;

        private Product hamburguer = null;
        private Product fries = null;
        private Product drink = null;
        private int step = 0;

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
                NextStep();
        }

        private void NextStep()
        {
            step++;
            SetGraphics();

            if (OrderComplete)
                Invoke(nameof(NewOrder), 1f);
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
            if (hamburguer != null && hamburguer.Ingredients.Count > step)
                return hamburguer.Ingredients[step];

            step -= HamburgerIngredientsCount;
            if (fries != null && fries.Ingredients.Count > step)
                return fries.Ingredients[step];

            step -= FriesIngredientsCount;
            if (drink != null && drink.Ingredients.Count > step)
                return drink.Ingredients[step];

            return null;
        }

        #endregion
    }
}
