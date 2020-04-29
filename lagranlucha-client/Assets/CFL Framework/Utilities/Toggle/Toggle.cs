using System;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace CFLFramework.Utilities
{
    public class Toggle : MonoBehaviour
    {
        #region FIELDS

        private const float ToggleDuration = 0.2f;

        [SerializeField] private Button button = null;
        [SerializeField] private Image switchBase = null;
        [SerializeField] private Color colorOn = Color.white;
        [SerializeField] private Color coloroff = Color.white;
        [SerializeField] private RectTransform knot = null;
        [SerializeField] private RectTransform positionOn = null;
        [SerializeField] private RectTransform positionOff = null;
        [SerializeField] private bool status = true;

        #endregion

        #region EVENTS

        public event Action<bool> onChange = null;

        #endregion

        #region PROPERTIES

        public bool Value { get => status; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            status = !status;
            switchBase.DOColor(status ? colorOn : coloroff, ToggleDuration);
            knot.DOAnchorPos(status ? positionOn.anchoredPosition : positionOff.anchoredPosition, ToggleDuration);
            onChange?.Invoke(status);
        }

        public void Initialize(bool status)
        {
            this.status = status;
            SetInitialStatus();
            onChange?.Invoke(status);
        }

        private void OnValidate()
        {
            SetInitialStatus();
        }

        private void SetInitialStatus()
        {
            if (switchBase != null)
                switchBase.color = status ? colorOn : coloroff;

            if (knot != null && positionOn != null && positionOff != null)
                knot.anchoredPosition = status ? positionOn.anchoredPosition : positionOff.anchoredPosition;
        }

        #endregion
    }
}
