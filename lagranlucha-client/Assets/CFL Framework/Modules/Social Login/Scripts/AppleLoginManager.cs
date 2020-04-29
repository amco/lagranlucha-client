using UnityEngine;
using System.Text;

using Zenject;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.IOS.NativeMessages;

using CFLFramework.API;

namespace CFLFramework.SocialLogin
{
    internal class AppleLoginManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] private APIManager apiManager = null;
        [Inject] private SocialLoginManager socialLoginManager = null;

        private IAppleAuthManager appleAuthManager = null;
        private IAppleIDCredential appleIdCredential = null;
        private IPasswordCredential passwordCredential = null;

        #endregion

        #region PROPERTIES

        internal string Id { get => appleIdCredential == null ? string.Empty : appleIdCredential.User; }
        internal string Token { get => appleIdCredential == null ? string.Empty : Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode); }
        internal string Email { get => appleIdCredential == null ? string.Empty : appleIdCredential.Email; }

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            if (Application.platform != RuntimePlatform.IPhonePlayer)
                return;

            if (AppleAuthManager.IsCurrentPlatformSupported)
            {
                var deserializer = new PayloadDeserializer();
                appleAuthManager = new AppleAuthManager(deserializer);
            }
        }

        private void Update()
        {
            if (appleAuthManager != null)
                appleAuthManager.Update();
        }

        internal void SignIn()
        {
            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail);
            this.appleAuthManager.LoginWithAppleId(loginArgs, SetCredentials, SignInError);
        }

        private void SetCredentials(ICredential credential)
        {
            socialLoginManager.OnSignInSucceeded();
            appleIdCredential = credential as IAppleIDCredential;
            passwordCredential = credential as IPasswordCredential;
            apiManager.LoginWithApple(Token, socialLoginManager.OnSocialLoginSucceeded);
        }

        private void SignInError(IAppleError error)
        {
            socialLoginManager.OnSignInError(error.ToString());
        }

        #endregion
    }
}
