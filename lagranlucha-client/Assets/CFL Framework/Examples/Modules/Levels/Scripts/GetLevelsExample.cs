using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Warnings;

namespace CFLFramework.Levels
{
    public class GetLevelsExample : MonoBehaviour
    {
        #region FIELDS

        private const string LevelDataFormat = "Name: {0}\nTag: {1}\nDescription: {2}\nDifficulty: {3}\n\n";
        private const string LevelsNotFoundMessage = "Levels not found.";

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
            Level[] levels = null;
            try
            {
                levels = levelsManager.GetLevels(inputField.text);
            }
            catch (LevelNotFoundException)
            {
                warningsManager.ShowWarning(LevelsNotFoundMessage);
            }

            if (levels == null || levels.Length <= 0)
                return;

            levelText.text = "";
            foreach (Level level in levels)
                levelText.text += string.Format(LevelDataFormat, level.Name, level.Tag, level.Description, level.Difficulty);
        }

        #endregion
    }
}
