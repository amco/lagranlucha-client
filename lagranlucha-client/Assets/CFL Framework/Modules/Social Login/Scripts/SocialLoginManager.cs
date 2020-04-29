using UnityEngine;
using UnityEngine.Events;

using Zenject;

using CFLFramework.API;

namespace CFLFramework.SocialLogin
{
    public class SocialLoginManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] private AppleLoginManager appleLoginManager = null;
        [Inject] private GoogleLoginManager googleLoginManager = null;
        [Inject] private APIManager apiManager = null;

        #endregion

        #region PROPERTIES

        public string Id { get => Application.platform == RuntimePlatform.Android ? googleLoginManager.Id : appleLoginManager.Id; }
        public string Token { get => Application.platform == RuntimePlatform.Android ? googleLoginManager.Token : appleLoginManager.Token; }
        public string Email { get => Application.platform == RuntimePlatform.Android ? googleLoginManager.Email : appleLoginManager.Email; }

        #endregion

        #region EVENTS

        public event UnityAction onSignInSucceeded;
        public event UnityAction<WebRequestResponse> onSocialLoginSucceeded;
        public event UnityAction<string> onSignInError;

        #endregion

        #region BEHAVIORS

        internal void OnSignInSucceeded()
        {
            onSignInSucceeded?.Invoke();
        }

        internal void OnSocialLoginSucceeded(WebRequestResponse response)
        {
            onSocialLoginSucceeded?.Invoke(response);
        }

        internal void OnSignInError(string error)
        {
            onSignInError?.Invoke(error);
        }

        public void SignIn()
        {
#if UNITY_ANDROID
            googleLoginManager.SignIn();
#elif UNITY_IOS
            appleLoginManager.SignIn();
#endif
        }

        public void Unlink()
        {
#if UNITY_ANDROID
            apiManager.UnlinkGoogleEmail();
#elif UNITY_IOS
            apiManager.UnlinkAppleEmail();
#endif
        }

        #endregion
    }
}
