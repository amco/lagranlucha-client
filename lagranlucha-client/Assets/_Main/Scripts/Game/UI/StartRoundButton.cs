using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace LaGranLucha.Game
{
    [RequireComponent(typeof(Button))]
    public class StartRoundButton : MonoBehaviour
    {
        #region FIELDS

        [Inject] private GameManager gameManager = null;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(StartRound);
        }

        private void StartRound()
        {
            gameManager.StartRound();
        }

        #endregion
    }
}
