using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CFLFramework.Score
{
    public class ScoreStructureSelectorExample : MonoBehaviour
    {
        #region FIELDS

        [Header("COMPONENTS")]
        [SerializeField] private InputField firstLevelInput = null;
        [SerializeField] private InputField secondLevelInput = null;

        #endregion

        #region BEHAVIORS

        protected string[] GetCurrentKeys()
        {
            List<string> keys = new List<string>();

            if (!string.IsNullOrEmpty(firstLevelInput.text))
                keys.Add(firstLevelInput.text);

            if (!string.IsNullOrEmpty(secondLevelInput.text))
                keys.Add(secondLevelInput.text);

            return keys.ToArray();
        }

        #endregion
    }
}
