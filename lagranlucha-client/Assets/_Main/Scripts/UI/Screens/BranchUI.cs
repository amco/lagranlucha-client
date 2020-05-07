using UnityEngine;
using UnityEngine.UI;

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

        [Header("BRANCH")]
        [SerializeField] private BranchHandler branchPrefab;
        [SerializeField] private Transform branchesContainer;

        [Header("UI")]
        [SerializeField] private Button backButton;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            backButton.onClick.AddListener(GoBackHome);
        }

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

        public void SelectBranch(Branch branch)
        {
            laGranLuchaManager.SelectBranch(branch);
            laGranLuchaManager.OpenShoppingScene();
            gameObject.SetActive(false);
            RemoveBranches();
        }

        private void GoBackHome()
        {
            laGranLuchaManager.OpenHomeScene();
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
