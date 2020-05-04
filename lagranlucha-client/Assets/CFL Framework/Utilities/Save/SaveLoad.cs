using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using Newtonsoft.Json;
using JsonApiSerializer;
using CFLFramework.Data;

namespace CFLFramework.Utilities.Save
{
    public static class SaveLoad
    {
        #region FIELDS

        private const string SavePath = "/saves/";
        private const string SavExtension = ".sav";
        private const string JsonExtension = ".json";

        #endregion

        #region BEHAVIORS

        public static void Save<T>(T objectToSave, string key)
        {
            string path = UnityEngine.Application.persistentDataPath + SavePath;
            Directory.CreateDirectory(path);
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(path + key + SavExtension, FileMode.Create))
                formatter.Serialize(fileStream, objectToSave);
        }

        public static void SaveJson<T>(T objectToSave, string key)
        {
            Directory.CreateDirectory(UnityEngine.Application.persistentDataPath + SavePath);
            string path = UnityEngine.Application.persistentDataPath + SavePath + key + JsonExtension;
            string jsonData = JsonUtility.ToJson(objectToSave);
            File.WriteAllText(path, jsonData);
        }

        public static T Load<T>(string key)
        {
            string path = UnityEngine.Application.persistentDataPath + SavePath;
            BinaryFormatter formatter = new BinaryFormatter();
            T returnValue = default(T);
            using (FileStream fileStream = new FileStream(path + key + SavExtension, FileMode.Open))
                returnValue = (T)formatter.Deserialize(fileStream);

            return returnValue;
        }

        public static T LoadJson<T>(string key)
        {
            string path = UnityEngine.Application.persistentDataPath + SavePath + key + JsonExtension;
            string dataAsJson = string.Empty;
            if (File.Exists(path))
                dataAsJson = File.ReadAllText(path);

            return JsonUtility.FromJson<T>(dataAsJson);
        }

        public static T LoadJsonFromStreamingAssets<T>(string key)
        {
            BetterStreamingAssets.Initialize();
            string dataAsJson = BetterStreamingAssets.ReadAllText(key + JsonExtension);
            return JsonUtility.FromJson<T>(dataAsJson);
        }

        public static Dictionary<string, object> LoadDictionaryFromStreamingAssets(string key)
        {
            BetterStreamingAssets.Initialize();
            string dataAsJson = BetterStreamingAssets.ReadAllText(key + JsonExtension);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(dataAsJson, new GenericConverter());
        }

        public static T LoadNewtonsoftJsonFromStreamingAssets<T>(string key)
        {
            BetterStreamingAssets.Initialize();
            string dataAsJson = BetterStreamingAssets.ReadAllText(key + JsonExtension);
            return JsonConvert.DeserializeObject<T>(dataAsJson, new JsonApiSerializerSettings());
        }

        public static void DeleteSave(string key)
        {
            if (SaveExists(key))
                File.Delete(UnityEngine.Application.persistentDataPath + SavePath + key + SavExtension);
        }

        public static void DeleteJson(string key)
        {
            if (JsonExists(key))
                File.Delete(UnityEngine.Application.persistentDataPath + SavePath + key + JsonExtension);
        }

        public static void DeleteAllSaveFiles()
        {
            string path = UnityEngine.Application.persistentDataPath + SavePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        public static bool SaveExists(string key)
        {
            return File.Exists(UnityEngine.Application.persistentDataPath + SavePath + key + SavExtension);
        }

        public static bool JsonExists(string key)
        {
            return File.Exists(UnityEngine.Application.persistentDataPath + SavePath + key + JsonExtension);
        }

        #endregion
    }
}
