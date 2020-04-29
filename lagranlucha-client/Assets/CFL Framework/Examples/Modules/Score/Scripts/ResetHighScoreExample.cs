using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Score
{
    [RequireComponent(typeof(Button))]
    public class ResetHighScoreExample : ScoreStructureSelectorExample
    {
        #region FIELDS

        [Inject] private ScoreManager scoreManager = null;

        private Button resetButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            resetButton = GetComponent<Button>();
            resetButton.onClick.AddListener(ResetHighScore);
        }

        private void ResetHighScore()
        {
            scoreManager.ResetHighScore(GetCurrentKeys());
        }

        #endregion
    }
}
