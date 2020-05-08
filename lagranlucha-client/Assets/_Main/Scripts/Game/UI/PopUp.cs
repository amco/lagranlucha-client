using UnityEngine;

using DG.Tweening;

namespace LaGranLucha.UI
{
    public class PopUp : MonoBehaviour
    {
        #region FIELDS

        private const float InitialScale = 0.25f;
        private const float ScaleDuration = 1f;
        private const float Overshoot = 1f;

        #endregion

        #region BEHAVIORS

        private void OnEnable()
        {
            transform.DOScale(Vector3.one * InitialScale, ScaleDuration).From().SetEase(Ease.OutElastic, Overshoot);
        }

        #endregion
    }
}
