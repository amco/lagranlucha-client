using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;
using CFLFramework.Splash;

namespace LaGranLucha.Splash
{
    public class SplashScreen : MonoBehaviour
    {
        #region FIELDS

        private const string MainScene = "Main";

        [Inject] private SplashModule splashModule;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            splashModule.StartSplash(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(MainScene);
        }

        #endregion
    }
}
