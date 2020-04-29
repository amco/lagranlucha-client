using System.Collections;
using UnityEngine;
using UnityEngine.Video;

using Zenject;

namespace CFLFramework.Splash
{
    public class SplashScreenVideo : MonoBehaviour, ISplashScreen
    {
        #region FIELDS

        [Inject] private SplashModule splashScreenController = null;

        [Header("COMPONENTS")]
        [SerializeField] private VideoPlayer videoPlayer = null;

        [Header("CONFIGURATION")]
        [SerializeField] private float waitDelay = 0.5f;

        #endregion

        #region BEHAVIORS

        public void Appear()
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += VideoEnded;
        }

        private void VideoEnded(VideoPlayer videoPlayer)
        {
            videoPlayer.Stop();
            StartCoroutine(WaitForEnd());
        }

        private IEnumerator WaitForEnd()
        {
            yield return new WaitForSeconds(waitDelay);
            splashScreenController.EndSplash();
        }

        #endregion
    }
}
