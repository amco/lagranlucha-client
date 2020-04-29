using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Zenject;

namespace CFLFramework.Audio
{
    public abstract class AudioToggle : MonoBehaviour
    {
        #region FIELDS

        [Inject] protected AudioManager audioManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button toggleButton = null;

        [Header("CONFIGURATION")]
        [SerializeField] private Sprite onSprite = null;
        [SerializeField] private Sprite offSprite = null;

        #endregion

        #region EVENTS

        protected event UnityAction toggle;
        protected event Func<bool> IsMuted;

        #endregion

        #region BEHAVIORS

        protected virtual void Start()
        {
            toggleButton.onClick.AddListener(ToggleButton);
            UpdateToggleSprite();
        }

        private void Reset()
        {
            toggleButton = GetComponentInChildren<Button>();
        }

        private void ToggleButton()
        {
            toggle?.Invoke();
            UpdateToggleSprite();
        }

        private void UpdateToggleSprite()
        {
            SetSprite(IsMuted() ? offSprite : onSprite);
        }

        private void SetSprite(Sprite sprite)
        {
            (toggleButton.targetGraphic as Image).sprite = sprite;
        }

        #endregion
    }
}
