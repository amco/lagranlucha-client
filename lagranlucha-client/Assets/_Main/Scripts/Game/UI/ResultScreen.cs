using UnityEngine;

using CFLFramework.Utilities.Extensions;
using Zenject;

namespace LaGranLucha.Game
{
    public class ResultScreen : MonoBehaviour
    {
        #region FIELDS

        [Inject] private GameManager gameManager = null;

        [SerializeField] private CanvasGroup resultScreen = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            gameManager.onRoundEnd += Show;
        }

        private void OnDestroy()
        {
            gameManager.onRoundEnd -= Show;
        }

        private void Show()
        {
            resultScreen.Show();
        }

        #endregion
    }
}
