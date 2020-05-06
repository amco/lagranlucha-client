using UnityEngine;
using UnityEngine.UI;

using Zenject;

using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public class LoginUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LaGranLuchaManager laGranLuchaManager;

        [Header("UI")]
        [SerializeField] private Button facebookButton;
        [SerializeField] private Button googleButton;
        [SerializeField] private Button appleButton;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            facebookButton.onClick.AddListener(LoginWithFacebook);
#if UNITY_ANDROID
            googleButton.gameObject.SetActive(true);
            googleButton.onClick.AddListener(LoginWithGoogle);
#else
            appleButton.gameObject.SetActive(true);
            appleButton.onClick.AddListener(LoginWithApple);
#endif
        }

        private void LoginWithFacebook()
        {

        }

        private void LoginWithGoogle()
        {

        }

        private void LoginWithApple()
        {

        }

        #endregion
    }
}
