using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Zenject;
using CFLFramework.API;
using CFLFramework.Utilities.Save;

namespace LaGranLucha.API
{
    public class BranchManager : MonoBehaviour
    {
        #region FIELDS

        private const string FakeBranchesJson = "branches";

        [Inject] private APIManager apiManager;

        private List<Branch> branches = new List<Branch>();

        #endregion

        #region PROPERTIES

        public Branch[] Branches { get => branches == null ? new Branch[] { } : branches.ToArray(); }

        #endregion

        #region BEHAVIORS

        public void GetBranches(UnityAction<WebRequestResponse> response)
        {
            List<Branch> branches = SaveLoad.LoadNewtonsoftJsonFromStreamingAssets<List<Branch>>(FakeBranchesJson);
            apiManager.SendFakeRequestSucceded(branches, (webResponse => PopulateList(ref branches, webResponse)) + response);
        }

        private void PopulateList<T>(ref List<T> list, WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            list = new List<T>(response.ResponseArray<T>());
        }

        #endregion
    }
}
