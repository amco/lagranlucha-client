namespace LaGranLucha.UI
{
    public class FoodHandler : ProductHandler
    {
        #region BEHAVIORS

        protected override void SetupUI()
        {
            nameText.text = product.Name;
            priceText.text = product.Price.ToString();
        }

        #endregion
    }
}
