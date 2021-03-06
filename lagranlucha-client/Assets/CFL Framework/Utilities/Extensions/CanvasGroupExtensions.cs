﻿using UnityEngine;

using DG.Tweening;

namespace CFLFramework.Utilities.Extensions
{
    public static class CanvasGroupExtensions
    {
        #region FIELDS

        private const float FadeDuration = 0.3f;
        private const float NoFade = 1f;
        private const float ClearFade = 0f;

        #endregion

        #region BEHAVIORS

        public static Tween Show(this CanvasGroup canvasGroup, float duration = FadeDuration)
        {
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = ClearFade;
            return canvasGroup.DOFade(NoFade, duration).OnComplete(() => canvasGroup.blocksRaycasts = true);
        }

        public static Tween Hide(this CanvasGroup canvasGroup, float duration = FadeDuration, bool disableOnEnd = true)
        {
            canvasGroup.blocksRaycasts = false;
            Tween hideTween = canvasGroup.DOFade(ClearFade, duration);
            if (disableOnEnd)
                hideTween.OnComplete(() => canvasGroup.gameObject.SetActive(false));

            return hideTween;
        }

        #endregion
    }
}
