using UnityEngine;
using UnityEngine.Events;

namespace CFLFramework.Splash
{
    public class SplashModule : MonoBehaviour
    {
        #region FIELDS

        [Header("COMPONENTS")]
        [SerializeField] private GameObject container = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private bool testMode = false;

        private ISplashScreen splashScreen = null;
        public event UnityAction onSplashEnded = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            splashScreen = GetComponentInChildren<ISplashScreen>();
        }

        public void StartSplash(UnityAction onSplashEnd)
        {
            this.onSplashEnded += onSplashEnd;

            if (testMode)
            {
                EndSplash();
                return;
            }

            splashScreen.Appear();
        }

        public void EndSplash()
        {
            onSplashEnded?.Invoke();
            container.SetActive(false);
        }

        #endregion
    }
}
