using UnityEngine.Events;

namespace CFLFramework.API
{
    public partial class APIManager
    {
        #region BEHAVIORS

        internal void SendEventCode(string eventCode, UnityAction<WebRequestResponse> response = null)
        {
            string endPoint = Endpoint.EventCodes + eventCode + Endpoint.Redeem;
            RequestContent requestContent = new RequestContent(RequestType.Post, endPoint);
            requester.SendRequest(this, requestContent, response);
        }

        #endregion      
    }
}
