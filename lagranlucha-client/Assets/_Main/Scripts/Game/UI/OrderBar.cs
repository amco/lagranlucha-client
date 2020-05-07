using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Zenject;

namespace LaGranLucha.Game
{
    public class OrderBar : MonoBehaviour
    {
        #region FIELDS

        private const float CompletelyFilledAmount = 1f;

        [Inject] private OrderManager orderManager = null;

        [SerializeField] private Image fill = null;

        private Tween barTween;

        #endregion

        #region PROPERTIES

        public bool OnTime { get => fill.fillAmount > default(int); }

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
            fill.fillAmount = CompletelyFilledAmount;
            barTween = fill.DOFillAmount(default(int), order.BarTime).SetEase(Ease.Linear);
        }

        #endregion
    }
}
