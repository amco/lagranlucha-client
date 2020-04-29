using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Utilities.Extensions;

namespace CFLFramework.Score
{
    public class ScoreUIExample : ScoreStructureSelectorExample
    {
        #region FIELDS

        private const string ScoreDefaultFormat = "{0:n0} pts";

        [Inject] private ScoreManager scoreManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Text scoreText = null;

        [Header("CONFIGURATION")]
        [SerializeField] private string scoreFormat = ScoreDefaultFormat;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            scoreManager.scoreUpdated += UpdateText;
        }

        private void OnDestroy()
        {
            scoreManager.scoreUpdated -= UpdateText;
        }

        private void Start()
        {
            string[] keys = GetCurrentKeys();

            UpdateText(keys.Length == 0 ? 0 : scoreManager.GetScore(GetCurrentKeys()));
        }

        private void UpdateText(string[] keys, float score)
        {
            if (!keys.IsEqual(GetCurrentKeys()))
                return;

            UpdateText(score);
        }

        private void UpdateText(float score)
        {
            scoreText.text = string.Format(scoreFormat, score);
        }

        #endregion
    }
}
