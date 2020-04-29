using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CFLFramework.Analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        #region BEHAVIORS

        private void Start()
        {
            Facebook.Unity.FB.Init();
        }

        public void Send(string eventName, bool facebookAnalytics = false, bool firebaseAnalytics = false, bool unityAnalytics = true, Dictionary<string, object> data = null)
        {
            if (unityAnalytics)
                SendToUnity(eventName, data);

            if (facebookAnalytics)
                SendToFacebook(eventName, data);

            if (firebaseAnalytics)
                SendToFirebase(eventName, data);
        }

        private void SendToUnity(string eventName, Dictionary<string, object> data = null)
        {
            UnityEngine.Analytics.Analytics.CustomEvent(eventName, data);
        }

        private void SendToFacebook(string eventName, Dictionary<string, object> data = null)
        {
            if (Facebook.Unity.FB.IsInitialized)
                Facebook.Unity.FB.LogAppEvent(eventName, parameters: data);
        }

        private void SendToFirebase(string eventName, Dictionary<string, object> data = null)
        {
            if (data == null)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
                return;
            }

            Firebase.Analytics.Parameter[] parameters = new Firebase.Analytics.Parameter[data.Count];
            string[] keys = data.Keys.ToArray();
            object[] values = data.Values.ToArray();

            for (int i = 0; i < data.Count; i++)
                if (values[i] is int)
                    parameters[i] = new Firebase.Analytics.Parameter(keys[i], (int)values[i]);
                else if (values[i] is float)
                    parameters[i] = new Firebase.Analytics.Parameter(keys[i], (float)values[i]);
                else
                    parameters[i] = new Firebase.Analytics.Parameter(keys[i], values[i].ToString());

            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameters);
        }

        #endregion
    }
}
