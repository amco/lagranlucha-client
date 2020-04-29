using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Utilities.Extensions;

namespace CFLFramework.Score
{
    public class HighScoreUIExample : ScoreStructureSelectorExample
    {
        #region FIELDS

        private const string HighScoreDefaultFormat = "High score: {0:n0}";

        [Inject] private ScoreManager scoreManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Text highScoreText = null;

        [Header("CONFIGURATION")]
        [SerializeField] private string highScoreFormat = HighScoreDefaultFormat;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            scoreManager.highScoreUpdated += UpdateText;
        }

        private void OnDestroy()
        {
            scoreManager.highScoreUpdated -= UpdateText;
        }

        private void Start()
        {
            string[] keys = GetCurrentKeys();

            UpdateText(keys.Length == 0 ? 0 : scoreManager.GetHighScore(GetCurrentKeys()));
        }

        private void UpdateText(string[] keys, float highScore)
        {
            if (!keys.IsEqual(GetCurrentKeys()))
                return;

            UpdateText(highScore);
        }

        private void UpdateText(float highScore)
        {
            highScoreText.text = string.Format(highScoreFormat, highScore);
        }

        #endregion
    }
}
