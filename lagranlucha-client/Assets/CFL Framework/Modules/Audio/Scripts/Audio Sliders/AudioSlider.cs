using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Zenject;

namespace CFLFramework.Audio
{
    public class AudioSlider : MonoBehaviour
    {
        #region FIELDS

        [Inject] protected AudioManager audioManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Image audioTypeImage = null;
        [SerializeField] protected Slider audioSlider = null;

        [Header("CONFIGURATION")]
        [SerializeField] private Sprite onSprite = null;
        [SerializeField] private Sprite offSprite = null;

        #endregion

        #region EVENTS

        protected UnityAction<float> newValueSelected;
        protected Func<bool> isMuted;

        #endregion

        #region BEHAVIORS

        protected virtual void Start()
        {
            audioSlider.onValueChanged.AddListener(SetNewValue);
            UpdateSprite();
        }

        private void Reset()
        {
            audioTypeImage = GetComponentInChildren<Image>();
            audioSlider = GetComponentInChildren<Slider>();
        }

        protected void SetNewValue(float newValue)
        {
            newValueSelected.Invoke(newValue / audioSlider.maxValue);
            UpdateSprite();
        }

        protected void UpdateSprite()
        {
            SetSprite(isMuted() ? offSprite : onSprite);
        }

        protected void UpdateSliderPosition(float value)
        {
            audioSlider.value = value;
        }

        private void SetSprite(Sprite sprite)
        {
            audioTypeImage.sprite = sprite;
        }

        #endregion
    }
}
