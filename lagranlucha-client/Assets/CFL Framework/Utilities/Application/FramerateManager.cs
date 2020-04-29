using UnityEngine;

namespace CFLFramework.Utilities.Application
{
    public class FramerateManager : MonoBehaviour
    {
        #region FIELDS

        private const int MinFramerate = 30;
        private const int MaxFramerate = 60;

        [Header("CONFIGURATIONS")]
        [SerializeField] [Range(MinFramerate, MaxFramerate)] private int framerate = MinFramerate;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            UnityEngine.Application.targetFrameRate = framerate;
        }

        #endregion
    }
}
