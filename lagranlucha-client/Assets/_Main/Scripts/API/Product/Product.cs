namespace LaGranLucha.API
{
    public abstract class Product
    {
        #region PROPERTIES

        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }

        #endregion
    }
}
