using UnityEngine;

using Zenject;
using CFLFramework.API;
using CFLFramework.Time;

using LaGranLucha.UI;
using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public class BranchManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LaGranLuchaManager laGranLuchaManager;
        [Inject] private TimeManager timeManager;

        [SerializeField] private BranchHandler branchPrefab;
        [SerializeField] private Transform branchesContainer;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            timeManager.onTimeLoaded += GetBranches;
        }

        private void OnDestroy()
        {
            timeManager.onTimeLoaded -= GetBranches;
        }

        private void GetBranches()
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

        #endregion
    }
}
