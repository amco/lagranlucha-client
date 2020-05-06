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

        [SerializeField] private List<Product> hamburguer = null;
        [SerializeField] private List<Product> fries = null;
        [SerializeField] private List<Product> drink = null;

        private Product selectedHamburguer = null;
        private Product selectedFries = null;
        private Product selectedDrink = null;

        #endregion

        #region PROPERTIES

        public Product Hamburguer { get => selectedHamburguer; }
        public Product Fries { get => selectedFries; }
        public Product Drink { get => selectedDrink; }
        public bool HaveHamburger { get => selectedHamburguer != null; }
        public bool HaveFries { get => selectedFries != null; }
        public bool HaveDrink { get => selectedDrink != null; }
        public int HamburgerIngredientsCount { get => selectedHamburguer != null ? selectedHamburguer.Ingredients.Count : 0; }
        public int FriesIngredientsCount { get => selectedFries != null ? selectedFries.Ingredients.Count : 0; }
        public int DrinkIngredientsCount { get => selectedDrink != null ? selectedDrink.Ingredients.Count : 0; }
        public string OrderName
        {
            get
            {
                List<string> names = new List<string>();
                if (HaveHamburger)
                    names.Add(selectedHamburguer.Name);

                if (HaveFries)
                    names.Add(selectedFries.Name);

                if (HaveDrink)
                    names.Add(selectedDrink.Name);

                return string.Format(ProductNameFormats[names.Count], names.ToArray());
            }
        }
        public float OrderScore
        {
            get
            {
                float points = 0;
                if (HaveHamburger)
                    points += selectedHamburguer.Points;

                if (HaveFries)
                    points += selectedFries.Points;

                if (HaveDrink)
                    points += selectedDrink.Points;

                return points;
            }
        }

        #endregion

        #region BEHAVIORS

        public void GetRandomElements()
        {
            selectedHamburguer = hamburguer.Count > 0 ? hamburguer[Random.Range(0, hamburguer.Count)] : null;
            selectedFries = fries.Count > 0 ? fries[Random.Range(0, fries.Count)] : null;
            selectedDrink = drink.Count > 0 ? drink[Random.Range(0, drink.Count)] : null;
        }

        #endregion
    }
}
