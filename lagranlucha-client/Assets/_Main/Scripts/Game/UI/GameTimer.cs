using System;
using UnityEngine;

using Zenject;

namespace LaGranLucha.Game
{
    public class GameTimer : MonoBehaviour
    {
        #region FIELDS

        private const float SecondDuration = 1f;

        [Inject] private GameManager gameManager = null;

        [SerializeField] private NumberCounter numberCounter = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            gameManager.onRoundStart += StartTimer;
        }

        private void OnDestroy()
        {
            gameManager.onRoundStart -= StartTimer;
        }

        private void StartTimer(int round)
        {
            numberCounter.Reset();
            InvokeRepeating(nameof(SecondPassed), SecondDuration, SecondDuration);
        }

        private void SecondPassed()
        {
            if (numberCounter.Value > default(int))
                numberCounter.Decrease();
            else
                CancelInvoke(nameof(SecondPassed));
        }

        #endregion
    }
}
