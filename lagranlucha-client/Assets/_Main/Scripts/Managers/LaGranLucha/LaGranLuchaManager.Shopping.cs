using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using CFLFramework.API;
using CFLFramework.Utilities.Save;

using LaGranLucha.API;

namespace LaGranLucha.Managers
{
    public partial class LaGranLuchaManager
    {
        #region FIELDS

        private const string FakeFoodsJson = "foods";
        private const string FakeDrinksJson = "drinks";

        [SerializeField] private GameObject shoppingScene;

        private List<Food> foods = new List<Food>();
        private List<Drink> drinks = new List<Drink>();

        #endregion

        #region PROPERTIES

        public List<Food> Foods { get => foods == null ? new List<Food>() : foods; }
        public List<Drink> Drinks { get => drinks == null ? new List<Drink>() { } : drinks; }

        #endregion

        #region BEHAVIORS

        public void GetFoods(UnityAction<WebRequestResponse> response)
        {
            List<Food> jsonFoods = SaveLoad.LoadNewtonsoftJsonFromStreamingAssets<List<Food>>(FakeFoodsJson);
            apiManager.SendFakeRequestSucceded(jsonFoods, (webResponse => PopulateList(ref foods, webResponse)) + response);
        }

        public void GetDrinks(UnityAction<WebRequestResponse> response)
        {
            List<Drink> jsonDrinks = SaveLoad.LoadNewtonsoftJsonFromStreamingAssets<List<Drink>>(FakeDrinksJson);
            apiManager.SendFakeRequestSucceded(jsonDrinks, (webResponse => PopulateList(ref drinks, webResponse)) + response);
        }

        public void OpenShoppingScene()
        {
            shoppingScene.SetActive(true);
        }

        #endregion
    }
}
