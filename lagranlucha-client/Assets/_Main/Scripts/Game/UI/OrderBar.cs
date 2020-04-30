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
        [SerializeField] private float fillDuration = 10f;

        private Tween barTween;

        #endregion

        #region PROPERTIES

        public float Percentage { get => barTween.ElapsedPercentage(false); }

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

        private void OrderStart()
        {
            fill.fillAmount = 1;
            barTween = fill.DOFillAmount(0, fillDuration).SetEase(Ease.Linear);
        }

        #endregion
    }
}
