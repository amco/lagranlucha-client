using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Score
{
    [RequireComponent(typeof(Button))]
    public class GainScoreExampleButton : ScoreStructureSelectorExample
    {
        #region FIELDS

        [Inject] private ScoreManager scoreManager = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private float scoreToGain = 0;

        private Button gainScoreButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            gainScoreButton = GetComponent<Button>();
            gainScoreButton.onClick.AddListener(GainScore);
        }

        private void GainScore()
        {
            string[] keys = GetCurrentKeys();

            if (keys.Length == 0)
                return;

            scoreManager.IncreaseScore(keys, scoreToGain);
        }

        #endregion
    }
}
