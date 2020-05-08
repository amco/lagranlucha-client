using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaGranLucha.Game
{
    public class GameManager : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private NumberCounter currentRound = null;

        #endregion

        #region EVENTS

        public event Action<int> onRoundStart;
        public event Action onRoundEnd;

        #endregion

        #region PROPERTIES

        public int CurrentRound { get => currentRound.IntValue; }

        #endregion

        #region BEHAVIORS

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            currentRound.Reset();
        }

        public void NextRound()
        {
            currentRound.Increase();
        }

        public void StartRound()
        {
            onRoundStart?.Invoke(currentRound.IntValue);
        }

        public void EndRound()
        {
            onRoundEnd?.Invoke();
        }

        #endregion
    }
}
