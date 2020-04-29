using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using CFLFramework.Utilities.Extensions;

namespace CFLFramework.Utilities.Touch
{
    public class TouchManager : MonoBehaviour
    {
        #region FIELDS

        [Header("PROPERTIES")]
        [SerializeField] [Range(0.1f, 1f)] private float clickRadius = 0.3f;
        [SerializeField] [Range(1f, 10f)] private float rayDistance = 2f;

        [Header("MASK")]
        [SerializeField] LayerMask touchableMasks = 0;
        [SerializeField] private string[] uiMasks;

        private GameObject lastObject;
        private bool isMoving = default(bool);

        #endregion

        #region EVENTS

        public event UnityAction<Vector2, GameObject> onMouseDown;
        public event UnityAction<Vector2, GameObject> onMouseDrag;
        public event UnityAction<Vector2, GameObject> onMouseUp;

        #endregion

        #region BEHAVIORS

        private void LateUpdate()
        {
            if (Input.touchSupported)
                HandleTouch();
            else
                HandleMouse();
        }

        public void SetMasks(string[] uiMasks)
        {
            this.uiMasks = uiMasks;
        }

        private void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMoving = !UIExtensions.IsClickingOverUI(Input.mousePosition, uiMasks);
                if (isMoving)
                {
                    lastObject = FindNearestObject(Input.mousePosition);
                    onMouseDown?.Invoke(Input.mousePosition, lastObject);
                }
            }

            if (Input.GetMouseButton(0) && isMoving)
                onMouseDrag?.Invoke(Input.mousePosition, lastObject);

            if (Input.GetMouseButtonUp(0))
            {
                isMoving = false;
                if (lastObject != null)
                    onMouseUp?.Invoke(Input.mousePosition, lastObject);
            }
        }

        private void HandleTouch()
        {
            if (Input.touchCount == 1)
            {
                UnityEngine.Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isMoving = !UIExtensions.IsClickingOverUI(touch.position, uiMasks);
                        if (isMoving)
                        {
                            lastObject = FindNearestObject(touch.position);
                            onMouseDown?.Invoke(touch.position, lastObject);
                        }

                        break;
                    case TouchPhase.Moved:
                        if (!isMoving)
                            return;

                        onMouseDrag?.Invoke(touch.position, lastObject);
                        break;
                    case TouchPhase.Ended:
                        isMoving = false;
                        if (lastObject != null)
                            onMouseUp?.Invoke(touch.position, lastObject);

                        break;
                }
            }
        }

        private GameObject FindNearestObject(Vector2 position)
        {
            Vector2 newPosition = Camera.main.ScreenToWorldPoint(position);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(newPosition, clickRadius, Vector2.zero, rayDistance, touchableMasks);
            Dictionary<GameObject, float> nearestObjects = new Dictionary<GameObject, float>();
            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    Vector2 objectOffset = newPosition - (Vector2)hit.transform.position;
                    if (!nearestObjects.ContainsKey(hit.transform.gameObject))
                        nearestObjects.Add(hit.transform.gameObject, objectOffset.magnitude);
                }

                KeyValuePair<GameObject, float> ordered = nearestObjects.OrderBy(distance => distance.Value).First();
                return ordered.Key;
            }

            return null;
        }

        #endregion
    }
}
