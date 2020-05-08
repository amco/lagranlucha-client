using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace LaGranLucha.Game
{
    [RequireComponent(typeof(Button))]
    public class IncreaseRoundButton : MonoBehaviour
    {
        #region FIELDS

        [Inject] private GameManager gameManager = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(IncreaseRound);
        }

        private void IncreaseRound()
        {
            gameManager.NextRound();
        }

        #endregion
    }
}
