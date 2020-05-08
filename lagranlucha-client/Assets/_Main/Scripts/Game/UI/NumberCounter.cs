using System;
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

        [SerializeField] private string id = "";
        [SerializeField] private float startingValue = 0;
        [SerializeField] private string prefix = "";
        [SerializeField] private string format = "{0:0.0}";
        [SerializeField] private string remove = ".0";
        [SerializeField] private string suffix = "";
        [SerializeField] private bool persistent = false;

        private TMP_Text number = null;
        private Tween scaleTween = null;

        private static Dictionary<string, Action<float>> localEvents = new Dictionary<string, Action<float>>();
        private static Dictionary<string, float> values = new Dictionary<string, float>();

        #endregion

        #region PROPERTIES

        public int IntValue { get => Mathf.RoundToInt(Value); }
        public string ValueString { get => prefix + string.Format(format, Value).Replace(remove, string.Empty) + suffix; }
        public float Value
        {
            get
            {
                if (!values.ContainsKey(id))
                    values.Add(id, persistent ? PlayerPrefs.GetFloat(id, startingValue) : startingValue);

                return values[id];
            }
            set
            {
                if (!values.ContainsKey(id))
                    values.Add(id, persistent ? PlayerPrefs.GetFloat(id, startingValue) : startingValue);

                values[id] = value;
            }
        }

        private Action<float> OnValueChange
        {
            get
            {
                if (!localEvents.ContainsKey(id))
                    localEvents.Add(id, null);

                return localEvents[id];
            }
            set
            {
                if (!localEvents.ContainsKey(id))
                    localEvents.Add(id, null);

                localEvents[id] = value;
            }
        }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            number = GetComponent<TMP_Text>();
            Value = startingValue;
            number.text = ValueString;
            OnValueChange += ValueChanged;
        }

        private void OnDestroy()
        {
            OnValueChange -= ValueChanged;
        }

        private void SetUI()
        {
            number.text = ValueString;
            if (scaleTween != null)
                scaleTween.Kill(true);

            scaleTween = transform.DOPunchScale(Vector3.one * ScaleAmount, ScaleDuration);
        }

        private void OnValidate()
        {
            localEvents = new Dictionary<string, Action<float>>();
            values = new Dictionary<string, float>();
            number = GetComponent<TMP_Text>();
            number.text = ValueString;
        }

        public void Reset()
        {
            values.Remove(id);
            OnValueChange?.Invoke(Value);
        }

        private void ValueChanged(float newValue)
        {
            Value = newValue;
            SetUI();

            if (persistent && PlayerPrefs.GetFloat(id) != Value)
                PlayerPrefs.SetFloat(id, Value);
        }

        public void Increase(float amount = 1)
        {
            if (amount == 0)
                return;

            OnValueChange?.Invoke(Value + amount);
        }

        public void Decrease(float amount = 1)
        {
            Increase(-amount);
        }

        public void Set(float newValue)
        {
            OnValueChange?.Invoke(newValue);
        }

        public void Subscribe(Action<float> action)
        {
            action?.Invoke(Value);
            OnValueChange += action;
        }

        public void Unsubscribe(Action<float> action)
        {
            if (this == null)
                return;

            OnValueChange -= action;
        }

        #endregion
    }
}
