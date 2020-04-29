using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Warnings;

namespace CFLFramework.Levels
{
    public class EraseLevelExample : MonoBehaviour
    {
        #region FIELDS

        private const string LevelNotErasedMessage = "Level could not be erased.";
        private const string LevelErasedMessage = "Level erased.";

        [Inject] private LevelsManager levelsManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button eraseLevelButton = null;
        [SerializeField] private InputField inputField = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            eraseLevelButton.onClick.AddListener(EraseLevel);
        }

        private void EraseLevel()
        {
            if (string.IsNullOrEmpty(inputField.text))
                return;

            try
            {
                levelsManager.EraseLevel(inputField.text);
                levelsManager.RefreshLevels();
                warningsManager.ShowWarning(LevelErasedMessage);
            }
            catch (LevelNotFoundException)
            {
                warningsManager.ShowWarning(LevelNotErasedMessage);
            }
        }

        #endregion
    }
}
