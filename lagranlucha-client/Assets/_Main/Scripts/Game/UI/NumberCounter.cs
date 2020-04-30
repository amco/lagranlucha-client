using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

namespace LaGranLucha.Game
{
    public class NumberCounter : MonoBehaviour
    {
        #region FIELDS

        private const float ScaleAmount = 0.5f;
        private const float ScaleDuration = 0.2f;

        [SerializeField] private float startingValue = 0;
        [SerializeField] private string suffix = "";

        private TMP_Text number = null;
        private Tween scaleTween = null;

        #endregion

        #region PROPERTIES

        public float Value { get; private set; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            number = GetComponent<TMP_Text>();
            Value = startingValue;
            number.text = Value.ToString() + suffix;
        }

        private void SetUI()
        {
            number.text = Value.ToString() + suffix;
            if (scaleTween != null)
                scaleTween.Kill(true);

            scaleTween = transform.DOPunchScale(Vector3.one * ScaleAmount, ScaleDuration);
        }

        public void Reset()
        {
            Value = startingValue;
        }

        public void Increase(float amount = 1)
        {
            if (amount == 0)
                return;

            Value += amount;
            SetUI();
        }

        public void Decrease(float amount = 1)
        {
            Increase(-amount);
        }

        public void Set(float newValue)
        {
            Value = newValue;
            SetUI();
        }

        private void OnValidate()
        {
            Awake();
        }

        #endregion
    }
}
