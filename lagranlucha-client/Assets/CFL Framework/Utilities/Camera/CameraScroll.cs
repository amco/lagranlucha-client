using UnityEngine;

using Zenject;
using CFLFramework.Utilities.Touch;

namespace CFLFramework.Utilities.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class CameraScroll : MonoBehaviour
    {
        #region FIELDS

        private const float MinDistanceMultiplier = 0.5f;

        [Inject] private TouchManager touchManager = null;

        [SerializeField] private float panSpeed = 1500f;
        [SerializeField] private bool hasYMovement = false;
        [SerializeField] private Transform limit = null;

        private new Camera camera;
        private Vector3 lastPanPosition;
        private Vector3 firstPoint;

        #endregion

        #region PROPERTIES

        public float XLinear { get => Mathf.InverseLerp(-XBound, XBound, camera.transform.position.x); }
        public float YLinear { get => Mathf.InverseLerp(-YBound, YBound, camera.transform.position.y); }
        private float XBound { get => limit.position.x - (camera.orthographicSize * camera.aspect); }
        private float YBound { get => hasYMovement ? limit.position.y - (camera.orthographicSize) : 0.0f; }
        private float XPosition { get => Mathf.Clamp(transform.position.x, -XBound, XBound); }
        private float YPosition { get => Mathf.Clamp(transform.position.y, -YBound, YBound); }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            camera = GetComponent<Camera>();

            touchManager.onMouseDown += MouseDown;
            touchManager.onMouseDrag += MouseDrag;
        }

        private void OnDestroy()
        {
            touchManager.onMouseDown -= MouseDown;
            touchManager.onMouseDrag -= MouseDrag;
        }

        private void MouseDown(Vector2 mousePosition, GameObject lastObject)
        {
            lastPanPosition = firstPoint = mousePosition;
        }

        private void MouseDrag(Vector2 mousePosition, GameObject lastObject)
        {
            PanCamera(mousePosition);
        }

        public void SetCameraXPosition(float linearTime)
        {
            float xPosition = Mathf.Lerp(-XBound, XBound, linearTime);
            transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
        }

        public void SetCameraYPosition(float linearTime)
        {
            float yPosition = Mathf.Lerp(-YBound, YBound, linearTime);
            transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
        }

        private void PanCamera(Vector3 newPanPosition)
        {
            Vector3 offset = camera.ScreenToViewportPoint(lastPanPosition - newPanPosition);
            Vector3 distanceMultiplier = camera.ScreenToViewportPoint(firstPoint - newPanPosition);
            Vector3 move = offset * Mathf.Max(Mathf.Abs(distanceMultiplier.x), MinDistanceMultiplier) * panSpeed * UnityEngine.Time.deltaTime;
            transform.Translate(move, Space.World);
            transform.position = new Vector3(XPosition, YPosition, transform.position.z);
            lastPanPosition = newPanPosition;
        }

        #endregion
    }
}
