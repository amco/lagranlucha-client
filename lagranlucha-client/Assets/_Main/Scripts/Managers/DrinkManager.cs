using CFLFramework.API;

using LaGranLucha.UI;
using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public class DrinkManager : ProductManager
    {
        #region BEHAVIORS

        private void Start()
        {
            GetDrinks();
        }

        protected override void OnGetProducts(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            foreach (Drink drink in laGranLuchaManager.Drinks)
                foreach (Variant variant in drink.Variants)
                    Instantiate<ProductHandler>(productPrefab, productsContainer).Initialize(variant);
        }

        #endregion
    }
}
