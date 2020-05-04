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

        [Inject] protected LaGranLuchaManager laGranLuchaManager;

        [SerializeField] protected ProductHandler productPrefab;
        [SerializeField] protected Transform productsContainer;

        #endregion

        #region BEHAVIORS

        protected void GetFoods()
        {
            laGranLuchaManager.GetFoods(OnGetProducts);
        }

        protected void GetDrinks()
        {
            laGranLuchaManager.GetDrinks(OnGetProducts);
        }

        protected abstract void OnGetProducts(WebRequestResponse response);

        #endregion
    }
}
