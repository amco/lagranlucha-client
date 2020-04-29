using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Warnings
{
    public class ShowWarningExample : MonoBehaviour
    {
        #region FIELDS

        [Inject] private WarningsManager warningsManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button showWarningButton = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private string warningMessage = "";

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            showWarningButton.onClick.AddListener(ShowWarning);
        }

        private void ShowWarning()
        {
            warningsManager.ShowWarning(warningMessage);
        }

        #endregion
    }
}
