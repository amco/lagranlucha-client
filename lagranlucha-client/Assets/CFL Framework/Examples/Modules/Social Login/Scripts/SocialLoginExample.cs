using UnityEngine;
using UnityEngine.UI;

using Zenject;

using CFLFramework.API;
using CFLFramework.Data;

namespace CFLFramework.SocialLogin
{
    public class SocialLoginExample : MonoBehaviour
    {
        #region FIELDS

        [Inject] private APIManager apiManager = null;
        [Inject] private DataManager dataManager = null;
        [Inject] private SocialLoginManager socialLoginManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Text isLoggedText = null;
        [SerializeField] private Text emailText = null;
        [SerializeField] private InputField idInput = null;
        [SerializeField] private InputField tokenInput = null;

        #endregion

        #region BEHAVIORS

        private void Update()
        {
            isLoggedText.text = "Is Logged: " + apiManager.IsLoggedIn;
            emailText.text = "Email: " + dataManager.User.Email;
            idInput.text = socialLoginManager.Id;
            tokenInput.text = socialLoginManager.Token;
        }

        #endregion
    }
}
