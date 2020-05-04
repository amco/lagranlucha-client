using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public class DrinkHandler : ProductHandler
    {
        #region FIELDS

        private Variant drink;

        #endregion

        #region BEHAVIORS

        public override void Initialize(Variant product)
        {
            drink = product;
            SetupUI();
        }

        protected override void SetupUI()
        {
            nameText.text = drink.Name;
            priceText.text = PesosSign + drink.Price.ToString();
        }

        #endregion

    }
}
