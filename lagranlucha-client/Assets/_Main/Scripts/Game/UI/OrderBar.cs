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

        private Tween barTween;

        #endregion

        #region PROPERTIES

        public bool OnTime { get => fill.fillAmount > 0; }

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
            barTween = fill.DOFillAmount(0, order.BarTime).SetEase(Ease.Linear);
        }

        #endregion
    }
}
