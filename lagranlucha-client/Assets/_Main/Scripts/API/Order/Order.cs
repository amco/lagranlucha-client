using System.Collections.Generic;

using Newtonsoft.Json;

namespace LaGranLucha.API
{
    public class Order
    {
        #region FIELDS

        private const string BranchIdBackend = "branch_id";
        private const string LineItemsBackend = "line_items";

        #endregion

        #region PROPERTIES

        public string Type { get; set; }
        public int Id { get; set; }
        [JsonProperty(BranchIdBackend)] public int BranchId { get; set; }
        [JsonProperty(LineItemsBackend)] public List<OrderProduct> LineItems { get; set; } = new List<OrderProduct>();

        #endregion
    }
}
