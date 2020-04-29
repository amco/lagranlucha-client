using UnityEngine;

using TMPro;

namespace CFLFramework.Utilities.ExternalLink
{
    public class TMProExternalLink : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private TMP_Text textMesh = null;
        [SerializeField] private string linkId = null;
        [SerializeField] private string linkToOpen = null;

        #endregion

        #region BEHAVIORS

        private void Update()
        {
            if (!Input.GetMouseButtonUp(0))
                return;

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMesh, Input.mousePosition, Camera.main);
            if (linkIndex > -1 && linkId == textMesh.textInfo.linkInfo[linkIndex].GetLinkID())
                UnityEngine.Application.OpenURL(linkToOpen);
        }

        #endregion
    }
}
