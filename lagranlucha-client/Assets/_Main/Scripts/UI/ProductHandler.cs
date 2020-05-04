using UnityEngine;
using UnityEngine.UI;

using TMPro;

using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public abstract class ProductHandler : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected TextMeshProUGUI priceText;
        [SerializeField] protected Image previewImage;

        protected Product product;

        #endregion

        #region BEHAVIORS

        public void Initialize(Product product)
        {
            this.product = product;
            SetupUI();
        }

        protected abstract void SetupUI();

        #endregion
    }
}
