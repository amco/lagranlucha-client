using UnityEngine;

using Zenject;

namespace CFLFramework.Splash
{
    public class SplashExample : MonoBehaviour
    {
        #region FIELDS

        private const string Message = "Splash ended";

        [Inject] private SplashModule splashModule = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            StartSplash();
        }

        private void StartSplash()
        {
            splashModule.StartSplash(ShowDebugMessage);
        }

        private void ShowDebugMessage()
        {
            Debug.Log(Message);
        }

        #endregion
    }
}
