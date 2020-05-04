using UnityEngine;

using CFLFramework.API;

using LaGranLucha.UI;
using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public class FoodManager : ProductManager
    {
        #region FIELDS

        [SerializeField] private FoodHandler foodPrefab;
        [SerializeField] private Transform foodsContainer;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            GetFoods();
        }

        protected override void OnGetProducts(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            foreach (Food food in apiManager.Foods)
                Instantiate<FoodHandler>(foodPrefab, foodsContainer).Initialize(food);
        }

        #endregion
    }
}
