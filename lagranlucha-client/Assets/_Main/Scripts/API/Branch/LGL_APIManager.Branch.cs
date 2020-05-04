using System.Collections.Generic;
using UnityEngine.Events;

using CFLFramework.API;
using CFLFramework.Utilities.Save;

namespace LaGranLucha.API
{
    public partial class LGL_APIManager
    {
        #region FIELDS

        private const string FakeBranchesJson = "branches";

        private List<Branch> branches = new List<Branch>();

        #endregion

        #region PROPERTIES

        public Branch[] Branches { get => branches == null ? new Branch[] { } : branches.ToArray(); }

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
