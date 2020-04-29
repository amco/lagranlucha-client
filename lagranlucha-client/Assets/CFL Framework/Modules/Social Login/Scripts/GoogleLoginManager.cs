using UnityEngine;
using UnityEngine.Events;

using Zenject;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

using CFLFramework.API;

namespace CFLFramework.SocialLogin
{
    internal class GoogleLoginManager : MonoBehaviour
    {
        #region FIELDS

#if UNITY_ANDROID
        private const string ErrorMessage = "Error found";

        [Inject] private APIManager apiManager = null;
        [Inject] private SocialLoginManager socialLoginManager = null;

        private PlayGamesLocalUser user = null;
        private UnityAction onSignInAction = null;
        private bool success = false;
#endif

        #endregion

        #region PROPERTIES

#if UNITY_ANDROID
        internal string Id { get => user == null ? string.Empty : user.id; }
        internal string Token { get => user == null ? string.Empty : user.GetIdToken(); }
        internal string Email { get => user == null ? string.Empty : user.Email; }
#else
        internal string Id { get => string.Empty; }
        internal string Token { get => string.Empty; }
        internal string Email { get => string.Empty; }
#endif

        #endregion

        #region BEHAVIORS

#if UNITY_ANDROID
        private void Start()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestIdToken().RequestEmail().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
        }

        private void Update()
        {
            onSignInAction?.Invoke();
        }

        internal void SignIn()
        {
            Social.localUser.Authenticate(OnAuthenticate);
        }

        private void OnAuthenticate(bool success)
        {
            this.success = success;
            onSignInAction = OnSignIn;
        }

        private void OnSignIn()
        {
            onSignInAction = null;

            if (success)
                SigninSucceeded();
            else
                socialLoginManager.OnSignInError(ErrorMessage);
        }

        private void SigninSucceeded()
        {
            user = Social.localUser as PlayGamesLocalUser;
            socialLoginManager.OnSignInSucceeded();
            apiManager.LoginWithGoogle(Token, socialLoginManager.OnSocialLoginSucceeded);
        }
#endif

        #endregion
    }
}
