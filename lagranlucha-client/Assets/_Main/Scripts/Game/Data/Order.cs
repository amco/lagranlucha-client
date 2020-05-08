using System.Collections.Generic;
using UnityEngine;

namespace LaGranLucha.Game
{
    [CreateAssetMenu(menuName = MenuName)]
    public class Order : ScriptableObject
    {
        #region FIELDS

        private const string MenuName = "La Gran Lucha/Game/Order";
        private const string OneProductFormat = "{0}";
        private const string TwoProductsFormat = "{0} y {1}";
        private const string ThreeProductsFormat = "Combo";
        private static readonly string[] ProductNameFormats = new string[] { string.Empty, OneProductFormat, TwoProductsFormat, ThreeProductsFormat };

        [SerializeField] private List<Product> hamburger = null;
        [SerializeField] private List<Product> fries = null;
        [SerializeField] private List<Product> drink = null;

        private Product selectedHamburger = null;
        private Product selectedFries = null;
        private Product selectedDrink = null;

        #endregion

        #region PROPERTIES

        public Product Hamburger { get => selectedHamburger; }
        public Product Fries { get => selectedFries; }
        public Product Drink { get => selectedDrink; }
        public bool HaveHamburger { get => selectedHamburger != null; }
        public bool HaveFries { get => selectedFries != null; }
        public bool HaveDrink { get => selectedDrink != null; }
        public int HamburgerIngredientsCount { get => selectedHamburger != null ? selectedHamburger.Ingredients.Count : 0; }
        public int HamburgerScore { get => HaveHamburger ? selectedHamburger.Points : 0; }
        public int FriesScore { get => HaveFries ? selectedFries.Points : 0; }
        public int DrinkScore { get => HaveDrink ? selectedDrink.Points : 0; }
        public int DelayedHamburgerScore { get => HaveHamburger ? selectedHamburger.DelayedPoints : 0; }
        public int DelayedFriesScore { get => HaveFries ? selectedFries.DelayedPoints : 0; }
        public int DelayedDrinkScore { get => HaveDrink ? selectedDrink.DelayedPoints : 0; }
        public string OrderName
        {
            get
            {
                List<string> names = new List<string>();
                if (HaveHamburger)
                    names.Add(selectedHamburger.Name);

                if (HaveFries)
                    names.Add(selectedFries.Name);

                if (HaveDrink)
                    names.Add(selectedDrink.Name);

                return string.Format(ProductNameFormats[names.Count], names.ToArray());
            }
        }

        public int OrderScore
        {
            get
            {
                int points = 0;
                points += HamburgerScore;
                points += FriesScore;
                points += DrinkScore;
                return points;
            }
        }

        public int DelayedOrderScore
        {
            get
            {
                int points = 0;
                points += DelayedHamburgerScore;
                points += DelayedFriesScore;
                points += DelayedDrinkScore;
                return points;
            }
        }

        public float BarTime
        {
            get
            {
                float estimatedTime = 0;
                estimatedTime += HaveHamburger ? selectedHamburger.EstimatedTime : 0;
                estimatedTime += HaveFries ? selectedFries.EstimatedTime : 0;
                estimatedTime += HaveDrink ? selectedDrink.EstimatedTime : 0;
                return estimatedTime;
            }
        }

        #endregion

        #region BEHAVIORS

        public void GetRandomElements()
        {
            selectedHamburger = hamburger.Count > 0 ? hamburger[Random.Range(0, hamburger.Count)] : null;
            selectedFries = fries.Count > 0 ? fries[Random.Range(0, fries.Count)] : null;
            selectedDrink = drink.Count > 0 ? drink[Random.Range(0, drink.Count)] : null;
        }

        #endregion
    }
}
