using CFLFramework.Data;
using Newtonsoft.Json;

namespace CFLFramework.Gifts
{
    public class Gift
    {
        #region FIELDS

        private const string BackendName = "gift";
        private const string RecipientIdProperty = "recipient_id";
        private const string GiftDataKey = "data";

        #endregion

        #region CONSTRUCTORS

        public Gift() { }

        public Gift(int friendId, string key, string action, int amount)
        {
            Type = BackendName;
            RecipientId = friendId;
            Key = key;
            Action = action;
            Data.Value = amount;
        }

        #endregion

        #region PROPERTIES

        public int Id { get; set; }
        public string Type { get; set; } = null;
        public string Status { get; set; } = null;
        public string Key { get; set; } = null;
        public string Action { get; set; } = null;
        public UserData Sender { get; set; } = null;
        public UserData Recipient { get; set; } = null;
        [JsonProperty(RecipientIdProperty)] public int RecipientId { get; set; }
        [JsonProperty(GiftDataKey)] public GiftData Data { get; set; } = new GiftData();

        #endregion
    }
}
