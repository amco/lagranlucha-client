using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening;

namespace CFLFramework.Store
{
    public class StoreConfirmationWindow : MonoBehaviour
    {
        #region FIELDS

        private const float FadeTransitionTime = 0.3f;

        [Header("COMPONENTS")]
        [SerializeField] private CanvasGroup windowCanvasGroup = null;
        [SerializeField] private Image thingToBuyImage = null;
        [SerializeField] private Text buyText = null;
        [SerializeField] private Image thingToSpendImage = null;
        [SerializeField] private Text spendText = null;
        [SerializeField] private Button confirmButton = null;
        [SerializeField] private Button closeButton = null;

        #endregion

        #region BEHAVIORS

        public virtual void Load(Sprite thingToSpend, string spendAmount, Sprite thingToBuy, string buyAmount, UnityAction onConfirmBuy)
        {
            thingToSpendImage.sprite = thingToSpend;
            thingToBuyImage.sprite = thingToBuy;
            spendText.text = spendAmount;
            buyText.text = buyAmount;
            confirmButton.onClick.AddListener(onConfirmBuy + Hide);
            closeButton.onClick.AddListener(Hide);
            Show();
        }

        protected virtual void Show()
        {
            windowCanvasGroup.DOFade(1, FadeTransitionTime);
        }

        protected virtual void Hide()
        {
            windowCanvasGroup.DOFade(0, FadeTransitionTime).OnComplete(() => DestroyWindow());
        }

        private void DestroyWindow()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}
