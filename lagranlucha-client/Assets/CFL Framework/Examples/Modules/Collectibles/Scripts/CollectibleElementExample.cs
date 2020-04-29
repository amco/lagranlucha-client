using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace CFLFramework.Collectibles
{
    public class CollectibleElementExample : MonoBehaviour
    {
        #region FIELDS

        private const string QuantityFormat = "{0:n0}";

        [Inject] private CollectiblesManager collectiblesManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Image collectibleImage = null;
        [SerializeField] private Text quantityText = null;
        [SerializeField] private Button increaseButton = null;
        [SerializeField] private Button spendButton = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private Collectible collectible = null;
        [SerializeField] private int increaseAmount = 8;
        [SerializeField] private int spendAmount = 10;

        #endregion

        #region BEHAVIORS

        private void Start()
        {
            collectible.updated += UpdateText;

            increaseButton.onClick.AddListener(IncreaseCollectible);
            spendButton.onClick.AddListener(SpendCollectible);

            UpdateSprite();
            UpdateText();
        }

        private void OnDestroy()
        {
            collectible.updated -= UpdateText;
        }

        private void UpdateSprite()
        {
            collectibleImage.sprite = collectible.Sprite;
        }

        private void UpdateText()
        {
            quantityText.text = string.Format(QuantityFormat, collectible.Quantity);
        }

        private void IncreaseCollectible()
        {
            collectiblesManager.IncreaseCollectible(collectible, increaseAmount);
        }

        private void SpendCollectible()
        {
            collectiblesManager.SpendCollectible(collectible, spendAmount);
        }

        #endregion
    }
}
