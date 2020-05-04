using System.Collections.Generic;
using UnityEngine.Events;

using CFLFramework.API;
using CFLFramework.Utilities.Save;

namespace LaGranLucha.API
{
    public partial class LaGranLuchaManager
    {
        #region FIELDS

        private const string FakeBranchesJson = "branches";

        private List<Branch> branches = new List<Branch>();

        #endregion

        #region PROPERTIES

        public List<Branch> Branches { get => branches == null ? new List<Branch>() : branches; }

        #endregion

        #region BEHAVIORS

        public void GetBranches(UnityAction<WebRequestResponse> response)
        {
            List<Branch> jsonBranches = SaveLoad.LoadNewtonsoftJsonFromStreamingAssets<List<Branch>>(FakeBranchesJson);
            apiManager.SendFakeRequestSucceded(jsonBranches, (webResponse => PopulateList(ref branches, webResponse)) + response);
        }

        #endregion
    }
}
