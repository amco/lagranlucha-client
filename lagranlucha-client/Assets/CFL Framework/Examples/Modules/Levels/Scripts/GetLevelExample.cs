using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Warnings;

namespace CFLFramework.Levels
{
    public class GetLevelExample : MonoBehaviour
    {
        #region FIELDS

        private const string LevelDataFormat = "Name: {0}\nTag: {1}\nDescription: {2}\nDifficulty: {3}";
        private const string LevelNotFoundMessage = "Level not found.";

        [Inject] private LevelsManager levelsManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button getLevelButton = null;
        [SerializeField] private Text levelText = null;
        [SerializeField] private InputField inputField = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            getLevelButton.onClick.AddListener(GetLevel);
        }

        private void GetLevel()
        {
            Level level = null;
            try
            {
                level = levelsManager.GetLevel(inputField.text);
            }
            catch (LevelNotFoundException)
            {
                warningsManager.ShowWarning(LevelNotFoundMessage);
            }

            if (level == null)
                return;

            levelText.text = string.Format(LevelDataFormat, level.Name, level.Tag, level.Description, level.Difficulty);
        }

        #endregion
    }
}
