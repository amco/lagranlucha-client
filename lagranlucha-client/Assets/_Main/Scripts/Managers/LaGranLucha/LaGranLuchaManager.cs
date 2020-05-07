using System.Collections.Generic;
using UnityEngine;

using Zenject;
using CFLFramework.API;

namespace LaGranLucha.Managers
{
    public partial class LaGranLuchaManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] private APIManager apiManager;

        #endregion

        #region BEHAVIORS

        private void PopulateList<T>(ref List<T> list, WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            list = new List<T>(response.ResponseArray<T>());
        }

        #endregion
    }
}
