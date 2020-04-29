using UnityEngine;

namespace CFLFramework.Warnings
{
    public class WarningsManager : MonoBehaviour
    {
        #region FIELDS

        [Header("COMPONENTS")]
        [SerializeField] private Warning warningPrefab = null;

        #endregion

        #region BEHAVIORS

        public void ShowWarning(string message)
        {
            Instantiate(warningPrefab).Appear(message);
        }

        public void ShowWarning(string message, params string[] data)
        {
            Instantiate(warningPrefab).Appear(string.Format(message, data));
        }

        #endregion
    }
}
