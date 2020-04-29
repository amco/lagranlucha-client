using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CFLFramework.Utilities.Extensions
{
    public static class RectTransformExtension
    {
        #region FIELDS

        private const int CornersInASquare = 4;
        private const int TopRightCorner = 2;

        #endregion

        #region BEHAVIORS

        public static void TakeScreenshotFromRectTransform(this RectTransform transformArea, MonoBehaviour behaviour, RenderMode renderMode, UnityAction<Texture2D> callback)
        {
            behaviour.StartCoroutine(TakeScreenshot(transformArea, renderMode, callback));
        }

        private static IEnumerator TakeScreenshot(RectTransform transformArea, RenderMode renderMode, UnityAction<Texture2D> callback)
        {
            yield return new WaitForEndOfFrame();
            bool isCameraOverlay = renderMode == RenderMode.ScreenSpaceOverlay;
            Vector3[] corners = new Vector3[CornersInASquare];
            transformArea.GetWorldCorners(corners);
            Vector3 bottomLeft = new Vector2(corners.First().x, corners.First().y);
            Vector3 bottomLeftCorners = isCameraOverlay ? bottomLeft : Camera.main.WorldToScreenPoint(bottomLeft);
            Vector3 topRight = new Vector2(corners[TopRightCorner].x, corners[TopRightCorner].y);
            Vector3 topRightCorners = isCameraOverlay ? topRight : Camera.main.WorldToScreenPoint(topRight);
            int width = (int)topRightCorners.x - (int)bottomLeftCorners.x;
            int height = (int)topRightCorners.y - (int)bottomLeftCorners.y;
            Texture2D texture = EasyMobile.Sharing.CaptureScreenshot(bottomLeftCorners.x, bottomLeftCorners.y, width, height);
            callback?.Invoke(texture);
        }

        #endregion
    }
}
