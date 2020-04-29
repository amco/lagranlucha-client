using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;
using TMPro;

namespace CFLFramework.Warnings
{
    public class Warning : MonoBehaviour
    {
        #region FIELDS

        [Header("COMPONENTS")]
        [SerializeField] private CanvasGroup warningCanvasGroup = null;
        [SerializeField] private Button warningButton = null;
        [SerializeField] private Text warningText = null;
        [SerializeField] private TextMeshProUGUI warningTextMeshPro = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private float fadeDuration = 0.25f;
        [SerializeField] private float duration = 2;

        #endregion

        #region BEHAVIORS

        public void Appear(string message)
        {
            warningButton.onClick.AddListener(DestroyWarning);

            if (warningText != null)
                warningText.text = message;

            if (warningTextMeshPro != null)
                warningTextMeshPro.text = message;

            warningCanvasGroup.DOFade(1, fadeDuration).OnComplete(() => StartCoroutine(Disappear()));
        }

        private IEnumerator Disappear()
        {
            yield return new WaitForSeconds(duration);
            warningCanvasGroup.DOFade(0, fadeDuration).OnComplete(() => DestroyWarning());
        }

        private void DestroyWarning()
        {
            Destroy(this.gameObject);
        }

        #endregion
    }
}
