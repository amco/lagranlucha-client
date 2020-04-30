using UnityEngine;

namespace LaGranLucha.Game
{
    public class GameTimer : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private NumberCounter numberCounter;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            InvokeRepeating(nameof(SecondPassed), 1f, 1f);
        }

        private void SecondPassed()
        {
            if (numberCounter.Value > 0)
                numberCounter.Decrease();
        }

        #endregion
    }
}
