using UnityEngine;

namespace LaGranLucha.Managers
{
    public partial class LaGranLuchaManager
    {
        #region FIELDS

        [SerializeField] private GameObject homeScene;

        #endregion

        #region BEHAVIORS

        public void OpenHomeScene()
        {
            homeScene.SetActive(true);
        }

        #endregion
    }
}
