using UnityEngine;

namespace CFLFramework.Utilities.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class CameraLockHorizontal : MonoBehaviour
    {
        #region FIELDS

        [Header("CONFIGURATIONS")]
        [SerializeField] private float desiredHorizontalSize = 10;
        [SerializeField] private bool frameCheck = false;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            SetCameraSize();
        }

        private void OnDrawGizmos()
        {
            SetCameraSize();
        }

        private void OnValidate()
        {
            SetCameraSize();
        }

        private void Update()
        {
            if (!frameCheck)
                return;

            SetCameraSize();
        }

        private void SetCameraSize()
        {
            Camera camera = GetComponent<Camera>();
            camera.orthographicSize = (desiredHorizontalSize / 2) / camera.aspect;
        }

        #endregion
    }
}
