using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using CFLFramework.API;
using CFLFramework.Utilities.Save;

using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public partial class LaGranLuchaManager
    {
        #region FIELDS

        private const string FakeBranchesJson = "branches";

        [SerializeField] private GameObject branchScene;

        private List<Branch> branches = new List<Branch>();

        #endregion

        #region PROPERTIES

        public Branch CurrentBranch { get; private set; }
        public List<Branch> Branches { get => branches == null ? new List<Branch>() : branches; }

        #endregion

        #region BEHAVIORS

        public void GetBranches(UnityAction<WebRequestResponse> response)
        {
            List<Branch> jsonBranches = SaveLoad.LoadNewtonsoftJsonFromStreamingAssets<List<Branch>>(FakeBranchesJson);
            apiManager.SendFakeRequestSucceded(jsonBranches, (webResponse => PopulateList(ref branches, webResponse)) + response);
        }

        public void SelectBranch(Branch branch)
        {
            CurrentBranch = branch;
        }

        public void OpenBranchScene()
        {
            branchScene.SetActive(true);
        }

        #endregion
    }
}
