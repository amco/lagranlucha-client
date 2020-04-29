using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Warnings;

namespace CFLFramework.Levels
{
    public class SaveLevelExample : MonoBehaviour
    {
        #region FIELDS

        private const string LevelNotFoundMessage = "Level not found.";
        private const string LevelNotSavedMessage = "Level could not saved.";
        private const string LevelSavedMessage = "Level saved.";

        [Inject] private LevelsManager levelsManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button saveLevelButton = null;
        [SerializeField] private InputField inputField = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            saveLevelButton.onClick.AddListener(SaveLevel);
        }

        private void SaveLevel()
        {
            if (string.IsNullOrEmpty(inputField.text))
                return;

            Level newLevel = new Level();
            newLevel.FileName = inputField.text;
            newLevel.Name = inputField.text;

            try
            {
                levelsManager.SaveLevel(newLevel, inputField.text);
                levelsManager.RefreshLevels();
                warningsManager.ShowWarning(LevelSavedMessage);
            }
            catch (LevelNotSavedException)
            {
                warningsManager.ShowWarning(LevelNotSavedMessage);
            }
        }

        #endregion
    }
}
