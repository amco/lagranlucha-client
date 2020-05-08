using UnityEngine;

namespace LaGranLucha.Managers
{
    public partial class LaGranLuchaManager
    {
        #region FIELDS

        [SerializeField] private GameObject loginScene;

        #endregion

        #region BEHAVIORS

        public void OpenLogin()
        {
            loginScene.SetActive(true);
        }

        #endregion
    }
}
