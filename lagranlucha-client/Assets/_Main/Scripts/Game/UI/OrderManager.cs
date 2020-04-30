using System.Collections.Generic;
using UnityEngine;

namespace LaGranLucha.Game
{
    public class OrderManager : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private Order[] possibleOrders = null;
        [SerializeField] private ShownIngredient[] shownIngredients = null;

        private List<Ingredient> hamburguerIngredients = null;
        private List<Ingredient> friesIngredients = null;
        private List<Ingredient> drinkIngredients = null;
        private int step = 0;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            NewOrder();
        }

        private void NewOrder()
        {
            Order newOrder = possibleOrders[Random.Range(0, possibleOrders.Length)];
            Debug.Log(newOrder.name);
            hamburguerIngredients = newOrder.Hamburguer.Count > 0 ? newOrder.Hamburguer[Random.Range(0, newOrder.Hamburguer.Count)].Ingredients : new List<Ingredient>();
            friesIngredients = newOrder.Fries.Count > 0 ? newOrder.Fries[Random.Range(0, newOrder.Fries.Count)].Ingredients : new List<Ingredient>();
            drinkIngredients = newOrder.Drink.Count > 0 ? newOrder.Drink[Random.Range(0, newOrder.Drink.Count)].Ingredients : new List<Ingredient>();
            step = 0;
            SetGraphics();
        }

        public void SendIngredient(Ingredient ingredient)
        {

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
            if (hamburguerIngredients.Count > step)
                return hamburguerIngredients[step];

            step -= hamburguerIngredients.Count;
            if (friesIngredients.Count > step)
                return friesIngredients[step];

            step -= friesIngredients.Count;
            if (drinkIngredients.Count > step)
                return drinkIngredients[step];

            return null;
        }

        #endregion
    }
}
