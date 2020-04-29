using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace CFLFramework.Store
{
    public class StoreNotAvailableWindow : MonoBehaviour
    {
        #region FIELDS

        private const float FadeTransitionTime = 0.3f;

        [Header("COMPONENTS")]
        [SerializeField] private CanvasGroup windowCanvasGroup = null;
        [SerializeField] private Button closeButton = null;

        #endregion

        #region BEHAVIORS

        public void Awake()
        {
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
