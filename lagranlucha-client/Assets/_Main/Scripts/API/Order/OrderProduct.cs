using Newtonsoft.Json;

namespace LaGranLucha.API
{
    public class OrderProduct
    {
        #region FIELDS

        private const string VariantIdBackend = "variant_id";

        #endregion

        #region PROPERTIES

        [JsonProperty(VariantIdBackend)] public int VariantId { get; set; }
        public int Quantity { get; set; }

        #endregion
    }
}
