namespace LaGranLucha.API
{
    public class Product
    {
        #region PROPERTIES

        public int Id { get; set; }
        public string Name { get; set; }
        public Variant[] Variants { get; set; }

        #endregion
    }
}
