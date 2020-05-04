using UnityEngine;
using UnityEngine.UI;

using TMPro;

using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public abstract class ProductHandler : MonoBehaviour
    {
        #region FIELDS

        protected const string PesosSign = "$";

        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected TextMeshProUGUI priceText;
        [SerializeField] protected Image previewImage;

        #endregion

        #region BEHAVIORS

        public abstract void Initialize(Product product);
        protected abstract void SetupUI();

        #endregion
    }
}
