using UnityEngine.Events;

using Zenject;
using Newtonsoft.Json;
using JsonApiSerializer;

using CFLFramework.SocialLogin;

namespace CFLFramework.API
{
    public partial class APIManager
    {
        #region BEHAVIORS

        internal void LoginWithGoogle(string token, UnityAction<WebRequestResponse> response)
        {
            LoginWithProvider(Endpoint.GoogleLogin, token, response);
        }

        internal void LoginWithApple(string token, UnityAction<WebRequestResponse> response)
        {
            LoginWithProvider(Endpoint.AppleLogin, token, response);
        }

        private void LoginWithProvider(string endPoint, string token, UnityAction<WebRequestResponse> response)
        {
            StopSynchronization();
            RequestContent requestContent = new RequestContent(RequestType.Post, endPoint);
            AccessToken accessToken = new AccessToken(token);
            requestContent.Content = JsonConvert.SerializeObject(accessToken, new JsonApiSerializerSettings());
            requester.SendRequest(this, requestContent, SaveAuthentication + response);
        }

        internal void UnlinkGoogleEmail(UnityAction<WebRequestResponse> response = null)
        {
            UnlinkEmail(Endpoint.GoogleUnlink, response);
        }

        internal void UnlinkAppleEmail(UnityAction<WebRequestResponse> response = null)
        {
            UnlinkEmail(Endpoint.AppleUnlink, response);
        }

        private void UnlinkEmail(string endPoint, UnityAction<WebRequestResponse> response)
        {
            ChangeToUserAuthentication();
            RequestContent requestContent = new RequestContent(RequestType.Post, string.Format(endPoint, requester.Authentication.Id));
            requestContent.Parameters.Add(PredicateKey.FieldUser, PredicateValue.Email);
            requester.SendRequest(this, requestContent, EmailUpdated + response);
        }

        #endregion
    }
}
