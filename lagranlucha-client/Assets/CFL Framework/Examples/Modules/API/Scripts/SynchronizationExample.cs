using UnityEngine;
using UnityEngine.UI;

using Zenject;
using CFLFramework.Warnings;
using CFLFramework.Data;

namespace CFLFramework.API
{
    public class SynchronizationExample : MonoBehaviour
    {
        #region FIELDS

        private static readonly string[] TestSynchronizationKeys = { "sync_test" };

        [Inject] private APIManager apiManager = null;
        [Inject] private WarningsManager warningsManager = null;
        [Inject] private DataManager dataManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button editDataButton = null;
        [SerializeField] private Button forceSyncButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            apiManager.onSynchronizationAttempt += SynchronizationAttempt;
            editDataButton.onClick.AddListener(ChangeTestData);
            forceSyncButton.onClick.AddListener(ForceSynchronization);
        }

        private void OnDestroy()
        {
            apiManager.onSynchronizationAttempt -= SynchronizationAttempt;
        }

        private void SynchronizationAttempt(WebRequestResponse response)
        {
            if (response.Succeeded)
                warningsManager.ShowWarning("Attempt succeeded!");
            else
                warningsManager.ShowWarning("Attempt failed...");
        }

        private void ForceSynchronization()
        {
            apiManager.SynchronizeUser(dataManager.TemporalUser, SynchronizationAttempt);
        }

        private void ChangeTestData()
        {
            dataManager.SetData(TestSynchronizationKeys, !dataManager.GetData<bool>(TestSynchronizationKeys, false));
        }

        #endregion
    }
}
