using UnityEngine;

using Zenject;
using CFLFramework.Warnings;
using CFLFramework.Data;

using I2LocalizationManager = I2.Loc.LocalizationManager;

namespace CFLFramework.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        #region FIELDS

        private const string CountryNotFoundMessage = "Country {0} not found.";
        private const string DefaultLanguage = "English";
        private const string LocalizationPreferencesKey = "localization_preferences";

        private static readonly string[] LanguageKeys = { LocalizationPreferencesKey, "language" };

        [Inject] private DataManager dataManager = null;
        [Inject] private WarningsManager warningsManager = null;

        #endregion

        #region PROPERTIES

        public string CurrentLanguageCode { get => I2LocalizationManager.CurrentLanguageCode; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            LoadPreferences();
        }

        private void LoadPreferences()
        {
            I2LocalizationManager.CurrentLanguage = dataManager.GetData<string>(LanguageKeys, DefaultLanguage);
        }

        private void SavePreferences()
        {
            dataManager.SetData(LanguageKeys, I2LocalizationManager.CurrentLanguage);
        }

        public void ChangeLanguage(string languageKey)
        {
            try
            {
                if (!I2LocalizationManager.HasLanguage(languageKey))
                    throw new LanguageNotFoundException();

                I2LocalizationManager.CurrentLanguage = languageKey;
                SavePreferences();
            }
            catch (LanguageNotFoundException)
            {
                warningsManager.ShowWarning(CountryNotFoundMessage, languageKey);
            }
        }

        public string GetTranslation(string term)
        {
            return I2LocalizationManager.GetTranslation(term);
        }

        public T GetTranslatedObject<T>(string term) where T : Object
        {
            return I2LocalizationManager.GetTranslatedObjectByTermName<T>(term);
        }

        #endregion
    }
}
