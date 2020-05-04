using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public class FoodHandler : ProductHandler
    {
        #region FIELDS

        private Food food;

        #endregion

        #region BEHAVIORS

        public override void Initialize(Product product)
        {
            food = (Food)product;
            SetupUI();
        }

        protected override void SetupUI()
        {
            nameText.text = food.Name;
            priceText.text = PesosSign + food.Price.ToString();
        }

        #endregion
    }
}
