using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using CFLFramework.Utilities.Extensions;

namespace CFLFramework.Utilities.Share
{
    public class ShareButton : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private RectTransform shareArea = null;
        [SerializeField] private RenderMode renderMode = RenderMode.ScreenSpaceCamera;
        [SerializeField] private Transform[] elementsToHide = null;

        private List<Vector3> temporalScale = new List<Vector3>();

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Share);
        }

        private void Share()
        {
            HideObjects();
            shareArea.TakeScreenshotFromRectTransform(this, renderMode, TextureObtained);
        }

        private void TextureObtained(Texture2D texture)
        {
            SharingExtension.ShareTexture(texture);
            ShowObjects();
        }

        private void HideObjects()
        {
            for (int i = 0; i < elementsToHide.Length; i++)
            {
                temporalScale.Add(elementsToHide[i].localScale);
                elementsToHide[i].localScale = Vector3.zero;
            }
        }

        private void ShowObjects()
        {
            for (int i = 0; i < elementsToHide.Length; i++)
                elementsToHide[i].localScale = temporalScale[i];

            temporalScale.Clear();
        }

        #endregion
    }
}
