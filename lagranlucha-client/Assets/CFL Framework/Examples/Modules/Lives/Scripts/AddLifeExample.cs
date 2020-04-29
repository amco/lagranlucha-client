using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Lives
{
    public class AddLifeExample : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LivesManager livesManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Button addLifeButton = null;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            addLifeButton.onClick.AddListener(AddLife);
        }

        private void AddLife()
        {
            livesManager.AddLives(1);
        }

        #endregion
    }
}
