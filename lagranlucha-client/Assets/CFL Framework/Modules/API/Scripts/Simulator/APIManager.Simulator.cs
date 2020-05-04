using System;
using UnityEngine.Events;

using NSubstitute;
using Newtonsoft.Json;
using JsonApiSerializer;

namespace CFLFramework.API
{
    public partial class APIManager
    {
        #region BEHAVIORS

        public void SendFakeRequestSucceded(Object dataResponse, UnityAction<WebRequestResponse> response)
        {
            requester.SendRequest(this, CreateFakeRequestContent(dataResponse), response);
        }

        private RequestContent CreateFakeRequestContent(Object dataResponse)
        {
            RequestContent requestContent = new RequestContent(RequestType.Get, string.Empty);
            requestContent.WebRequest = Substitute.For<IWebRequest>();
            requestContent.WebRequest.ResponseCode.Returns((long)(HttpStatusCode.Ok));
            requestContent.Content = JsonConvert.SerializeObject(dataResponse, new JsonApiSerializerSettings());
            requestContent.WebRequest.RawResponse.Returns(requestContent.Content);
            return requestContent;
        }

        #endregion
    }
}
