using System.Collections.Generic;
using UnityEngine.Events;

using CFLFramework.API;
using CFLFramework.Utilities.Save;

namespace LaGranLucha.API
{
    public partial class LGL_APIManager
    {
        #region FIELDS

        private const string FakeFoodsJson = "foods";
        private const string FakeDrinksJson = "drinks";

        private List<Food> foods = new List<Food>();
        private List<Drink> drinks = new List<Drink>();

        #endregion

        #region PROPERTIES

        public Food[] Foods { get => foods == null ? new Food[] { } : foods.ToArray(); }
        public Drink[] Drinks { get => drinks == null ? new Drink[] { } : drinks.ToArray(); }

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

        #endregion
    }
}
