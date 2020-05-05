using UnityEngine;
using UnityEngine.UI;

using CFLFramework.Utilities.Extensions;

namespace LaGranLucha.Game
{
    [RequireComponent(typeof(Button))]
    public class ShowOnClick : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private CanvasGroup canvasGroup = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            canvasGroup.Show();
        }

        #endregion
    }
}
