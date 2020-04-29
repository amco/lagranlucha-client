using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace CFLFramework.API
{
    public class Requester
    {
        #region FIELDS

        private const string InitParameters = "?";
        private const string TokenHeader = "X-User-Token";
        private const string UsernameLogin = "X-User-Login";
        private const string SecretTokenHeader = "X-Game-Secret-Token";

        private string host = null;
        private string secretToken = null;
        private UnityAction<WebRequestResponse> unauthorizedResponse = null;

        #endregion

        #region CONSTRUCTOR

        public Requester(string host, Authentication authentication, string secretToken, UnityAction<WebRequestResponse> unauthorizedResponse)
        {
            this.host = host;
            this.Authentication = authentication;
            this.secretToken = secretToken;
            this.unauthorizedResponse = unauthorizedResponse;
        }

        #endregion

        #region PROPERTIES

        public Authentication Authentication { get; private set; }

        #endregion

        #region BEHAVIORS

        public void SendRequest(MonoBehaviour coroutineOwner, RequestContent requestContent, UnityAction<WebRequestResponse> response)
        {
            coroutineOwner.StartCoroutine(Request(requestContent, response));
        }

        private IEnumerator Request(RequestContent requestContent, UnityAction<WebRequestResponse> response)
        {
            string parametres = requestContent.Parameters.ToQueryString();
            string url = string.IsNullOrEmpty(parametres) ? GetURL(requestContent) : GetURL(requestContent) + InitParameters + parametres;
            if (secretToken != null)
                requestContent.Headers.Add(SecretTokenHeader, secretToken);

            if (Authentication != null)
            {
                requestContent.Headers.Add(TokenHeader, Authentication.Token);
                requestContent.Headers.Add(UsernameLogin, Authentication.Email != null ? Authentication.Email : Authentication.Username);
            }

            IWebRequest webRequest = requestContent.WebRequest;
            webRequest.SetUnityWebRequest(new UnityWebRequest(url, requestContent.RequestType));
            webRequest.SetDownloadHandler(new DownloadHandlerBuffer());
            if (!string.IsNullOrEmpty(requestContent.Content))
            {
                IWebUploadHandler webUploadHandler = requestContent.WebUploadHandler;
                webUploadHandler.AddContent(requestContent.Content);
                webRequest.SetUploadHandler(webUploadHandler.UploadHandler);
            }

            foreach (KeyValuePair<string, string> header in requestContent.Headers)
                if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Value))
                    webRequest.SetRequestHeader(header.Key, header.Value);

            yield return webRequest.SendWebRequest();

            var webRequestResponse = new WebRequestResponse(webRequest);

            if (webRequestResponse.StatusCode == HttpStatusCode.Unauthorized)
                unauthorizedResponse?.Invoke(webRequestResponse);
            else
                response?.Invoke(webRequestResponse);
        }

        private string GetURL(RequestContent requestContent)
        {
            return host + requestContent.Endpoint;
        }

        public void ResetAuthentication()
        {
            Authentication = null;
        }

        public void SaveAuthentication(Authentication newAuthentication)
        {
            Authentication = newAuthentication;
        }

        #endregion
    }
}
