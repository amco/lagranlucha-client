using UnityEngine.Events;

using Newtonsoft.Json;
using JsonApiSerializer;

namespace CFLFramework.API
{
    public partial class APIManager
    {
        #region BEHAVIORS

        internal void SearchUser(string userName, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Get, Endpoint.Users);
            requestContent.Parameters.Add(PredicateKey.FieldUser, PredicateValue.Username);
            requestContent.Parameters.Add(PredicateKey.QueryUsernameContains, userName);
            requestContent.Parameters.Add(PredicateKey.QueryUsernameNotEqual, requester.Authentication.Username);
            requester.SendRequest(this, requestContent, response);
        }

        internal void GetFriends(UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Get, Endpoint.Friends);
            requestContent.Parameters.Add(PredicateKey.FieldFriend, PredicateValue.Username);
            requester.SendRequest(this, requestContent, response);
        }

        internal void GetFriendRequests(UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Get, Endpoint.FriendRequests);
            requestContent.Parameters.Add(PredicateKey.Include, PredicateValue.Sender);
            requestContent.Parameters.Add(PredicateKey.FieldSender, PredicateValue.Username);
            requestContent.Parameters.Add(PredicateKey.QueryStatusEqual, PredicateValue.Pending);
            requestContent.Parameters.Add(PredicateKey.QuerySenderUsernameNotEqual, requester.Authentication.Username);
            requester.SendRequest(this, requestContent, response);
        }

        internal void DeleteFriend(int friendId, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Delete, Endpoint.FriendsId + friendId);
            requester.SendRequest(this, requestContent, response);
        }

        internal void CreateFriendRequest(int friendId, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Post, Endpoint.FriendRequests);
            FriendRequest friendRequest = new FriendRequest();
            friendRequest.RecipientId = friendId;
            requestContent.Content = JsonConvert.SerializeObject(friendRequest, new JsonApiSerializerSettings());
            requester.SendRequest(this, requestContent, response);
        }

        internal void AcceptFriendRequest(int friendRequestId, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Post, Endpoint.FriendRequestsId + friendRequestId + Endpoint.FriendRequestsAccept);
            requester.SendRequest(this, requestContent, response);
        }

        internal void RejectFriendRequest(int friendRequestId, UnityAction<WebRequestResponse> response)
        {
            RequestContent requestContent = new RequestContent(RequestType.Post, Endpoint.FriendRequestsId + friendRequestId + Endpoint.FriendRequestsReject);
            requester.SendRequest(this, requestContent, response);
        }

        #endregion      
    }
}
