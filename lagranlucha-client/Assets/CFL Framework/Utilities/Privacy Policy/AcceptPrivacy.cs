using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CFLFramework.Utilities.PrivacyPolicy
{
    public class AcceptPrivacy : MonoBehaviour
    {
        #region FIELDS

        private const string PrivacyKey = "PrivacyPolicy";

        [Header("COMPONENTS")]
        [SerializeField] private Button acceptPrivacyButton = null;
        [SerializeField] private Button openPrivacyButton = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private string privacyLink = null;
        [SerializeField] private int privacyIndex = default(int);

        #endregion

        #region EVENTS

        public event UnityAction onPrivacyPolicyAccepted;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            acceptPrivacyButton.onClick.AddListener(Accept);
            openPrivacyButton.onClick.AddListener(OpenPrivacy);
        }

        private void Start()
        {
            CheckPrivacy();
        }

        public void CheckPrivacy()
        {
            bool accepted = PlayerPrefsExtension.GetBool(PrivacyKey + privacyIndex);

            if (!accepted)
                return;

            onPrivacyPolicyAccepted?.Invoke();
            Destroy(gameObject);
        }

        private void Accept()
        {
            PlayerPrefsExtension.SetBool(PrivacyKey + privacyIndex, true);
            onPrivacyPolicyAccepted?.Invoke();
            Destroy(gameObject);
        }

        private void OpenPrivacy()
        {
            UnityEngine.Application.OpenURL(privacyLink);
        }

        #endregion
    }
}
