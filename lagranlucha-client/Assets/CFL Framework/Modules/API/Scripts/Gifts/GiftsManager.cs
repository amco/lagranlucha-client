using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

using Zenject;
using Newtonsoft.Json;
using CFLFramework.API;
using CFLFramework.Data;
using CFLFramework.Collectibles;

namespace CFLFramework.Gifts
{
    public class GiftsManager : MonoBehaviour
    {
        #region FIELDS

        public const string GiftsSaveDataKey = "Gifts";
        private const string DefaultGiftAction = "sum";

        [Inject] private APIManager apiManager = null;
        [Inject] private DataManager dataManager = null;
        [InjectOptional] private CollectiblesManager collectiblesManager = null;

        private List<Gift> currentGifts = new List<Gift>();

        #endregion

        #region PROPERTIES

        public Gift[] CurrentGifts { get => currentGifts.ToArray(); }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            LoadLocalGiftsData();
        }

        public void SendGift(int friendId, string[] keys, int amount, UnityAction<WebRequestResponse> response)
        {
            apiManager.CreateGift(friendId, keys, DefaultGiftAction, amount, response);
        }

        public void RedeemGift(Gift gift, UnityAction<WebRequestResponse> response)
        {
            apiManager.RedeemGift(gift, GiftRedeemed + response);
        }

        private void GiftRedeemed(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            var redeemedGift = response.Response<Gift>();
            dataManager.MergeData(redeemedGift.Recipient.Data);
            currentGifts.RemoveAll(gift => gift.Id == redeemedGift.Id);
            SaveLocalGiftsData();
            collectiblesManager?.ForceCollectiblesUpdate();
        }

        public void GetGifts(UnityAction<WebRequestResponse> response)
        {
            apiManager.GetGifts(FillGiftList + response);
        }

        private void FillGiftList(WebRequestResponse response)
        {
            if (!response.Succeeded)
                return;

            currentGifts = new List<Gift>(response.ResponseArray<Gift>());
            SaveLocalGiftsData();
        }

        private void SaveLocalGiftsData()
        {
            PlayerPrefs.SetString(GiftsSaveDataKey, JsonConvert.SerializeObject(currentGifts, Formatting.Indented));
        }

        private void LoadLocalGiftsData()
        {
            if (!PlayerPrefs.HasKey(GiftsSaveDataKey))
                return;

            currentGifts = JsonConvert.DeserializeObject<List<Gift>>(PlayerPrefs.GetString(GiftsSaveDataKey), new GenericConverter());
        }

        #endregion
    }
}
