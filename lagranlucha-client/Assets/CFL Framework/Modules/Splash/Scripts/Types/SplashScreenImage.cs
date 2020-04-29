using System.Collections;
using UnityEngine;

using Zenject;
using DG.Tweening;

namespace CFLFramework.Splash
{
    public class SplashScreenImage : MonoBehaviour, ISplashScreen
    {
        #region FIELDS

        [Inject] private SplashModule splashScreenController = null;

        [Header("COMPONENTS")]
        [SerializeField] private CanvasGroup canvasGroup = null;

        [Header("CONFIGURATION")]
        [SerializeField] private float fadeDuration = 0.75f;
        [SerializeField] private float logoDelay = 2f;
        [SerializeField] private float waitDelay = 0.2f;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            InstantDissapear();
        }

        public void Appear()
        {
            canvasGroup.DOFade(1, fadeDuration).OnComplete(() => StartCoroutine(Disappear()));
        }

        private IEnumerator Disappear()
        {
            yield return new WaitForSeconds(logoDelay);
            canvasGroup.DOFade(0, fadeDuration).OnComplete(() => StartCoroutine(WaitForEnd()));
        }

        private IEnumerator WaitForEnd()
        {
            yield return new WaitForSeconds(waitDelay);
            splashScreenController.EndSplash();
        }

        private void InstantDissapear()
        {
            canvasGroup.alpha = 0;
        }

        #endregion
    }
}
