using UnityEngine;

using Zenject;
using CFLFramework.API;

using LaGranLucha.UI;
using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public abstract class ProductManager : MonoBehaviour
    {
        #region FIELDS

        [Inject] protected LGL_APIManager apiManager;

        [SerializeField] protected ProductHandler productPrefab;
        [SerializeField] protected Transform productsContainer;

        #endregion

        #region BEHAVIORS

        protected void GetFoods()
        {
            apiManager.GetFoods(OnGetProducts);
        }

        protected void GetDrinks()
        {
            apiManager.GetDrinks(OnGetProducts);
        }

        protected abstract void OnGetProducts(WebRequestResponse response);

        #endregion
    }
}
