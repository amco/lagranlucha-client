using UnityEngine;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace CFLFramework.Data
{
    public class DataManager : MonoBehaviour
    {
        #region FIELDS

        public const string UserDataKey = "UserData";
        public const string TemporalUserDataKey = "TemporalUserData";

        #endregion

        #region PROPERTIES

        public UserData User { get; private set; }
        public UserData TemporalUser { get; private set; }

        public bool IsSynchronizationPending { get => TemporalUser.SomethingChanged; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            LoadLocalData();
        }

        private void LoadLocalData()
        {
            User = JsonConvert.DeserializeObject<UserData>(PlayerPrefs.GetString(UserDataKey), new GenericConverter());
            TemporalUser = JsonConvert.DeserializeObject<UserData>(PlayerPrefs.GetString(TemporalUserDataKey), new GenericConverter());

            if (User == null)
                User = new UserData();

            if (TemporalUser == null)
                TemporalUser = new UserData();

            SaveLocalData();
        }

        public void ResetUser()
        {
            User = new UserData();
            TemporalUser = new UserData();
            SaveLocalData();
        }

        public void ResetTemporalUser()
        {
            TemporalUser = new UserData();
            SaveLocalData();
        }

        private void SaveLocalData()
        {
            PlayerPrefs.SetString(UserDataKey, SerializeUser(User));
            PlayerPrefs.SetString(TemporalUserDataKey, SerializeUser(TemporalUser));
        }

        public void DeleteUserData()
        {
            PlayerPrefs.DeleteKey(UserDataKey);
            PlayerPrefs.DeleteKey(TemporalUserDataKey);
        }

        private string SerializeUser(object user)
        {
            return JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public void SetUser(UserData user)
        {
            if (user == null)
                return;

            User = user;
            TemporalUser = new UserData();
            SaveLocalData();
        }

        public void SetAuthentication(int authenticationId, string authenticationUsername, string authenticationEmail)
        {
            User = new UserData(User, authenticationId.ToString(), authenticationUsername, authenticationEmail);
            SaveLocalData();
        }

        public void LinkEmail(string email)
        {
            User = new UserData(User, email);
            SaveLocalData();
        }

        public void MergeData(Dictionary<string, object> dataDictionary)
        {
            User.MergeData(dataDictionary);
            TemporalUser.MergeData(dataDictionary);
            SaveLocalData();
        }

        public void SetData<T>(string[] keys, T data)
        {
            User.SetData(keys, data);
            TemporalUser.SetData(keys, data);
            SaveLocalData();
        }

        public void SetExperience(int experience)
        {
            User.Experience = experience.ToString();
            TemporalUser.Experience = experience.ToString();
            SaveLocalData();
        }

        public void SetRank(int rank)
        {
            User.Rank = rank.ToString();
            TemporalUser.Rank = rank.ToString();
            SaveLocalData();
        }

        public void SetLevel(int level)
        {
            User.Level = level.ToString();
            TemporalUser.Level = level.ToString();
            SaveLocalData();
        }

        public List<T> GetDataList<T>(UserData user, string[] keys, object defaultValue)
        {
            return user.GetDataList<T>(keys, defaultValue);
        }

        public List<T> GetDataList<T>(string[] keys, object defaultValue)
        {
            return User.GetDataList<T>(keys, defaultValue);
        }

        public T GetData<T>(UserData user, string[] keys, object defaultValue = null)
        {
            return user.GetData<T>(keys, defaultValue);
        }

        public T GetData<T>(string[] keys, object defaultValue = null)
        {
            return User.GetData<T>(keys, defaultValue);
        }

        public bool HasData(string[] keys)
        {
            return User.HasData(keys);
        }

        public int GetExperience()
        {
            return int.Parse(User.Experience);
        }

        public int GetRank()
        {
            return int.Parse(User.Rank);
        }

        public int GetLevel()
        {
            return int.Parse(User.Level);
        }

        public static void ResetKey(string[] keys, object newValue)
        {
            DataManager dataManager = new GameObject().AddComponent<DataManager>().GetComponent<DataManager>();
            dataManager.LoadLocalData();
            dataManager.SetData<object>(keys, newValue);
            DestroyImmediate(dataManager.gameObject);
        }

        public static string[] GenerateKeys(params string[] keys)
        {
            return keys;
        }

        public static string[] GenerateKeys(string[] prefixKeys, params string[] keys)
        {
            List<string> keyList = new List<string>(prefixKeys);
            keyList.AddRange(keys);
            return keyList.ToArray();
        }

        public static void DeleteUsers()
        {
            PlayerPrefs.DeleteKey(UserDataKey);
            PlayerPrefs.DeleteKey(TemporalUserDataKey);
        }

        #endregion
    }
}
