using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Lives
{
    public class SpendLifeExample : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LivesManager livesManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button spendLifeButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            spendLifeButton.onClick.AddListener(SpendLife);
        }

        private void SpendLife()
        {
            livesManager.SpendLife();
        }

        #endregion
    }
}
