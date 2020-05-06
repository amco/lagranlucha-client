using UnityEngine;
using UnityEngine.UI;

using Zenject;

using LaGranLucha.Managers;

namespace LaGranLucha.UI
{
    public class HomeUI : MonoBehaviour
    {
        #region FIELDS

        [Inject] private LaGranLuchaManager laGranLuchaManager;

        [Header("UI")]
        [SerializeField] private Button customizeOrderButton;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            customizeOrderButton.onClick.AddListener(CustomizeOrder);
        }

        public void CustomizeOrder()
        {
            laGranLuchaManager.OpenShoppingScene();
            gameObject.SetActive(false);
        }

        #endregion
    }
}
