using UnityEngine;
using UnityEngine.UI;

using Zenject;

using CFLFramework.API;
using CFLFramework.Data;

namespace CFLFramework.SocialLogin
{
    public class SocialLoginButton : MonoBehaviour
    {
        #region FIELDS

        private const string TestEmailFormat = "{0}@test.test";
        private const int TestEmailMaxNumber = 9999999;

        [Inject] private APIManager apiManager = null;
        [Inject] private DataManager dataManager = null;
        [Inject] private SocialLoginManager socialLoginManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button socialLoginButton = null;
        [SerializeField] private Image socialLoginIcon = null;
        [SerializeField] private Text socialLoginText = null;

        [Header("GOOGLE CONFIGURATIONS")]
        [SerializeField] private Sprite googleSprite = null;
        [SerializeField] private string googleLoginMessage = null;

        [Header("APPLE CONFIGURATIONS")]
        [SerializeField] private Sprite appleSprite = null;
        [SerializeField] private string appleLoginMessage = null;

        [Header("TESTING CONFIGURATIONS")]
        [SerializeField] private string overrideMail = null;

        #endregion

        #region PROPERTIES

        private bool UserAlreadySignedIn { get => apiManager.IsLoggedIn && !string.IsNullOrEmpty(dataManager.User.Email); }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            socialLoginManager.onSocialLoginSucceeded += OnUserLoginSuccessfully;
            socialLoginManager.onSignInSucceeded += OnSignInSuccess;
            socialLoginManager.onSignInError += OnError;
        }

        private void OnDestroy()
        {
            socialLoginManager.onSocialLoginSucceeded -= OnUserLoginSuccessfully;
            socialLoginManager.onSignInSucceeded -= OnSignInSuccess;
            socialLoginManager.onSignInError -= OnError;
        }

        private void Start()
        {
            InitializePlatformUI();
        }

        private void InitializePlatformUI()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                socialLoginIcon.sprite = googleSprite;
                socialLoginButton.image.color = Color.white;
                socialLoginText.color = Color.black;
                socialLoginText.text = googleLoginMessage;
            }
            else
            {
                socialLoginIcon.sprite = appleSprite;
                socialLoginButton.image.color = Color.black;
                socialLoginText.color = Color.white;
                socialLoginText.text = appleLoginMessage;
            }

            socialLoginButton.onClick.AddListener(SocialLoginButtonHandler);
        }

        private void SocialLoginButtonHandler()
        {
            Disable();
            if (Application.isEditor)
                LinkRandomEmail();
            else
                socialLoginManager.SignIn();
        }

        private void LinkRandomEmail()
        {
            string email = !string.IsNullOrEmpty(overrideMail) ? overrideMail : string.Format(TestEmailFormat, Random.Range(default(int), TestEmailMaxNumber));
            apiManager.LinkEmail(email, socialLoginManager.OnSocialLoginSucceeded);
        }

        private void OnUserLoginSuccessfully(WebRequestResponse response)
        {
            Enable();
        }

        private void OnSignInSuccess()
        {
            Enable();
        }

        private void Disable()
        {
            socialLoginButton.interactable = false;
        }

        private void Enable()
        {
            socialLoginButton.interactable = true;
        }

        private void OnError(string error)
        {
            Enable();
        }

        #endregion
    }
}
