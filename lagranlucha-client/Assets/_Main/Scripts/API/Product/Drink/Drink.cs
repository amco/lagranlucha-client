namespace LaGranLucha.API
{
    public class Drink
    {
        #region PROPERTIES

        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Variant[] Variants { get; set; }

        #endregion
    }
}
