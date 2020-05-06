using UnityEngine;

using Zenject;
using CFLFramework.API;

using LaGranLucha.API;
using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public class BranchUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LaGranLuchaManager laGranLuchaManager;

        [SerializeField] private BranchHandler branchPrefab;
        [SerializeField] private Transform branchesContainer;

        #endregion

        #region BEHAVIORS

        private void OnEnable()
        {
            laGranLuchaManager.GetBranches(OnGetBranches);
        }

        private void OnGetBranches(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            foreach (Branch branch in laGranLuchaManager.Branches)
                Instantiate<BranchHandler>(branchPrefab, branchesContainer).Initialize(branch);
        }

        public void SelectBranch(int branchId)
        {
            laGranLuchaManager.SelectBranch(branchId);
            laGranLuchaManager.OpenShoppingScene();
            gameObject.SetActive(false);
            RemoveBranches();
        }

        private void RemoveBranches()
        {
            foreach (Transform child in branchesContainer)
                Destroy(child.gameObject);
        }

        #endregion
    }
}
