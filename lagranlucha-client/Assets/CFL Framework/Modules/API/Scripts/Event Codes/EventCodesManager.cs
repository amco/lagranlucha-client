using UnityEngine;
using UnityEngine.Events;

using Zenject;
using CFLFramework.API;

namespace CFLFramework.EventCodes
{
    public class EventCodesManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] private APIManager apiManager = null;

        #endregion

        #region BEHAVIORS

        public void SendCode(string code, UnityAction<EventCode> onEventCodeReceived, UnityAction<WebRequestResponse> response = null)
        {
            apiManager.SendEventCode(code, (webResponse => PopulateEventCode(webResponse, onEventCodeReceived)) + response);
        }

        private void PopulateEventCode(WebRequestResponse response, UnityAction<EventCode> onEventCodeReceived)
        {
            if (!response.Succeeded)
                return;

            onEventCodeReceived?.Invoke(response.Response<EventCode>());
        }

        #endregion
    }
}
