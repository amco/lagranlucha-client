using CFLFramework.API;

using LaGranLucha.UI;
using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public class FoodManager : ProductManager
    {
        #region BEHAVIORS

        private void Start()
        {
            GetFoods();
        }

        protected override void OnGetProducts(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            foreach (Food food in laGranLuchaManager.Foods)
                foreach (Variant variant in food.Variants)
                Instantiate<CartItemHandler>(productPrefab, productsContainer).Initialize(variant);
        }

        #endregion
    }
}
