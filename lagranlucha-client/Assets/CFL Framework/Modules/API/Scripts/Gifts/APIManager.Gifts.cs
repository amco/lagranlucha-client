using UnityEngine.Events;

using Newtonsoft.Json;
using JsonApiSerializer;

using CFLFramework.Gifts;

namespace CFLFramework.API
{
    public partial class APIManager
    {
        #region FIELDS

        private const string KeyJoinCharacter = ".";

        #endregion

        #region BEHAVIORS

        internal void RedeemGift(Gift gift, UnityAction<WebRequestResponse> response = null)
        {
            if (!IsLoggedIn)
                return;

            string endPoint = Endpoint.GiftsId + gift.Id + Endpoint.Redeem;
            RequestContent requestContent = new RequestContent(RequestType.Post, endPoint);
            requestContent.Parameters.Add(PredicateKey.Include, PredicateValue.Recipient);
            requestContent.Parameters.Add(PredicateKey.FieldRecipient, GetFullGiftJoinedKey(gift.Key));
            requestContent.Parameters.Add(PredicateKey.FieldGift, PredicateValue.Recipient);
            requester.SendRequest(this, requestContent, response);
        }

        internal void CreateGift(int friendId, string[] keys, string action, int amount, UnityAction<WebRequestResponse> response = null)
        {
            if (!IsLoggedIn)
                return;

            RequestContent requestContent = new RequestContent(RequestType.Post, Endpoint.Gifts);
            Gift gift = new Gift(friendId, GetGiftJoinedKey(keys), action, amount);
            requestContent.Content = JsonConvert.SerializeObject(gift, new JsonApiSerializerSettings());
            requester.SendRequest(this, requestContent, response);
        }

        internal void GetGifts(UnityAction<WebRequestResponse> response = null)
        {
            if (!IsLoggedIn)
                return;

            RequestContent requestContent = new RequestContent(RequestType.Get, Endpoint.Gifts);
            requestContent.Parameters.Add(PredicateKey.QueryRecipientIdEqual, dataManager.User.Id.ToString());
            requestContent.Parameters.Add(PredicateKey.QueryStatusEqual, PredicateValue.Pending);
            requestContent.Parameters.Add(PredicateKey.Include, PredicateValue.Sender);
            requestContent.Parameters.Add(PredicateKey.FieldUser, PredicateValue.Username);
            requester.SendRequest(this, requestContent, response);
        }

        private string GetGiftJoinedKey(string[] keys)
        {
            return string.Join(KeyJoinCharacter, keys);
        }

        private string GetFullGiftJoinedKey(string giftKey)
        {
            return PredicateValue.Data + KeyJoinCharacter + giftKey;
        }

        #endregion      
    }
}
