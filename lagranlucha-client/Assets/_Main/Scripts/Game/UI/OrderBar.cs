using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Zenject;

namespace LaGranLucha.Game
{
    public class OrderBar : MonoBehaviour
    {
        #region FIELDS

        [Inject] private OrderManager orderManager = null;

        [SerializeField] private Image fill = null;
        [SerializeField] private float fillDuration = 20f;
        [SerializeField] private AnimationCurve penaltyCurve = null;
        [SerializeField] private float[] penaltyAmount = null;
        [SerializeField] private Color[] penaltyColor = null;

        private Tween barTween;

        #endregion

        #region PROPERTIES

        public float GetPenalty { get => penaltyAmount[Mathf.RoundToInt(penaltyCurve.Evaluate(fill.fillAmount))]; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            orderManager.onNewOrderStart += OrderStart;
            orderManager.onOrderCompleted += OrderCompleted;
        }

        private void OrderCompleted()
        {
            barTween.Pause();
        }

        private void OrderStart(Order order)
        {
            fill.fillAmount = 1;
            barTween = fill.DOFillAmount(0, fillDuration).SetEase(Ease.Linear).OnUpdate(SetBarColor);
        }

        private void SetBarColor()
        {
            fill.color = penaltyColor[Mathf.RoundToInt(penaltyCurve.Evaluate(fill.fillAmount))];
        }

        #endregion
    }
}
