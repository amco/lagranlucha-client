using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public class FoodHandler : ProductHandler
    {
        #region FIELDS

        private Variant food;

        #endregion

        #region BEHAVIORS

        public override void Initialize(Variant product)
        {
            food = product;
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
