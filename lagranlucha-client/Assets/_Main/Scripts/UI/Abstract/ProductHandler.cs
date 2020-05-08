using UnityEngine;
using UnityEngine.UI;

using TMPro;

using LaGranLucha.API;

namespace LaGranLucha.UI
{
    public abstract class ProductHandler : MonoBehaviour
    {
        #region FIELDS

        [Header("PROPERTIES")]
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected TextMeshProUGUI priceText;
        [SerializeField] protected Image previewImage;

        protected Variant product;

        #endregion

        #region BEHAVIORS

        protected abstract void SetupUI();

        #endregion
    }
}
