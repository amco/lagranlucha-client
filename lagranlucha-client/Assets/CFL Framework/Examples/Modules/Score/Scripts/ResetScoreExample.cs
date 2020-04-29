using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Score
{
    [RequireComponent(typeof(Button))]
    public class ResetScoreExample : ScoreStructureSelectorExample
    {
        #region FIELDS

        [Inject] private ScoreManager scoreManager = null;

        private Button resetButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            resetButton = GetComponent<Button>();
            resetButton.onClick.AddListener(ResetScore);
        }

        private void ResetScore()
        {
            scoreManager.ResetScore(GetCurrentKeys());
        }

        #endregion
    }
}
