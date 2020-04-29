using Newtonsoft.Json;
using CFLFramework.Data;

namespace CFLFramework.API
{
    public class FriendRequest
    {
        #region FIELDS

        private const string SenderIdBackend = "sender_id";
        private const string RecipientIdBackend = "recipient_id";

        #endregion

        #region PROPERTIES

        public int Id { get; set; }
        [JsonProperty(propertyName: SenderIdBackend)] public int SenderId { get; set; }
        [JsonProperty(propertyName: RecipientIdBackend)] public int RecipientId { get; set; }
        public UserData Sender { get; set; }

        #endregion
    }
}
