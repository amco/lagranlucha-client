using UnityEngine;
using UnityEngine.UI;

namespace CFLFramework.Utilities.ExternalLink
{
    public class OpenExternalLink : MonoBehaviour
    {
        #region FIELDS

        [Header("COMPONENTS")]
        [SerializeField] private Button openExternalLinkButton = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private string link = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            openExternalLinkButton.onClick.AddListener(OpenLink);
        }

        private void OpenLink()
        {
            UnityEngine.Application.OpenURL(link);
        }

        #endregion
    }
}
