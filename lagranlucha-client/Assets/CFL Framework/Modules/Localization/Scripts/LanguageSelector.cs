using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Localization
{
    public class LanguageSelector : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LocalizationManager localizationManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button chooseCountryButton = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private string languageKey = "Spanish";

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            chooseCountryButton.onClick.AddListener(ButtonHandler);
        }

        private void Reset()
        {
            chooseCountryButton = GetComponentInChildren<Button>();
        }

        private void ButtonHandler()
        {
            localizationManager.ChangeLanguage(languageKey);
        }

        #endregion
    }
}
