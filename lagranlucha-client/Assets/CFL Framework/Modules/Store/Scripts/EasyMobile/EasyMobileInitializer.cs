using UnityEngine;

namespace EasyMobile
{
    public class EasyMobileInitializer : MonoBehaviour
    {
        #region BEHAVIORS

        private void Awake()
        {
            if (!RuntimeManager.IsInitialized())
                RuntimeManager.Init();
        }

        #endregion
    }
}
