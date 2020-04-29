using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CFLFramework.Utilities.Extensions
{
    public static class UIExtensions
    {
        #region BEHAVIORS

        public static bool IsClickingOverUI(Vector2 mousePosition, string[] detectLayers)
        {
            PointerEventData cursor = new PointerEventData(EventSystem.current);
            cursor.position = mousePosition;
            List<RaycastResult> objectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(cursor, objectsHit);
            for (int x = 0; x < objectsHit.Count; x++)
                if (detectLayers.Contains<string>(LayerMask.LayerToName(objectsHit[x].gameObject.layer)))
                    return true;

            return false;
        }

        #endregion
    }
}
