using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace CFLFramework.Data
{
    public class UserData
    {
        #region FIELDS

        private const string IdKey = "id";
        private const string TypeKey = "type";
        private const string UsernameKey = "username";
        private const string EmailKey = "email";
        private const string RankKey = "rank";
        private const string LevelKey = "level";
        private const string ExperienceKey = "xp";
        private const string DataKey = "data";
        private const string UserType = "user";

        #endregion

        #region PROPERTIES

        [JsonProperty(IdKey)] public string Id { get; private set; } = null;
        [JsonProperty(TypeKey)] public string Type { get; set; } = UserType;
        [JsonProperty(UsernameKey)] public string Username { get; private set; } = null;
        [JsonProperty(EmailKey)] public string Email { get; private set; } = null;
        [JsonProperty(RankKey)] public string Rank { get; internal set; } = null;
        [JsonProperty(LevelKey)] public string Level { get; internal set; } = null;
        [JsonProperty(ExperienceKey)] public string Experience { get; internal set; } = null;
        [JsonProperty(DataKey)] public Dictionary<string, object> Data { get; private set; } = null;

        internal bool SomethingChanged { get => Email != null || Username != null || Rank != null || Level != null || Experience != null || Data.Count > 0; }

        #endregion

        #region CONSTRUCTORS

        public UserData() : this(null, null, null, null, null, null, new Dictionary<string, object>()) { }

        public UserData(string username) : this(null, username, null, null, null, null, new Dictionary<string, object>()) { }

        public UserData(string id, string username) : this(id, username, null, null, null, null, new Dictionary<string, object>()) { }

        public UserData(UserData user, string email) : this(user.Id, user.Username, email, user.Level, user.Rank, user.Experience, user.Data) { }

        public UserData(UserData user, string id, string username, string email) : this(id, username, email, user.Level, user.Rank, user.Experience, user.Data) { }

        [JsonConstructor]
        public UserData(string id, string username, string email, string level, string rank, string experience, Dictionary<string, object> data)
        {
            Id = id;
            Username = username;
            Email = email;
            Level = level;
            Rank = rank;
            Experience = experience;
            Data = data;
        }

        #endregion

        #region BEHAVIORS

        internal void SetData<T>(string[] keys, T data)
        {
            SetValueOnDictionary(keys, data);
        }

        internal void MergeData(Dictionary<string, object> newDictionary)
        {
            MergeWithDataDictionary(newDictionary, new List<string>());
        }

        private void MergeWithDataDictionary(object newElement, List<string> keys)
        {
            if (newElement.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> element in (Dictionary<string, object>)newElement)
                {
                    var newKeys = new List<string>(keys);
                    newKeys.Add(element.Key);
                    MergeWithDataDictionary(element.Value, newKeys);
                }
            }
            else
            {
                SetValueOnDictionary(keys.ToArray(), newElement);
            }
        }

        private void SetValueOnDictionary(string[] keys, object newData)
        {
            var data = Data;
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!data.ContainsKey(keys[i]))
                    data.Add(keys[i], new Dictionary<string, object>());

                data = (Dictionary<string, object>)data[keys[i]];
            }

            if (data.ContainsKey(keys.Last()))
                data[keys.Last()] = newData;
            else
                data.Add(keys.Last(), newData);
        }

        internal List<T> GetDataList<T>(string[] keys, object defaultValue)
        {
            var list = GetData<object>(keys, defaultValue);
            if (list.GetType() == typeof(List<T>))
                return (List<T>)list;
            else
                return ConvertToList<T>(list);
        }

        internal T GetData<T>(string[] keys, object defaultValue)
        {
            Dictionary<string, object> dictionary = Data;
            defaultValue = defaultValue == null ? default(T) : defaultValue;

            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!dictionary.ContainsKey(keys[i]))
                    return (T)CastObject(defaultValue);

                if (dictionary[keys[i]].GetType() != typeof(Dictionary<string, object>))
                    return (T)CastObject(defaultValue);

                dictionary = dictionary[keys[i]] as Dictionary<string, object>;
            }

            if (!dictionary.ContainsKey(keys.Last()))
                return (T)CastObject(defaultValue);

            return (T)CastObject(dictionary[keys.Last()]);
        }

        internal bool HasData(string[] keys)
        {
            Dictionary<string, object> dictionary = Data;

            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!dictionary.ContainsKey(keys[i]))
                    return false;

                if (dictionary[keys[i]].GetType() != typeof(Dictionary<string, object>))
                    return false;

                dictionary = dictionary[keys[i]] as Dictionary<string, object>;
            }

            return dictionary.ContainsKey(keys.Last());
        }

        private List<T> ConvertToList<T>(object originalList)
        {
            var newList = new List<T>();
            foreach (object element in (List<object>)originalList)
                newList.Add((T)Convert.ChangeType(element, typeof(T)));

            return newList;
        }

        private object CastObject(object data)
        {
            if (data.GetType() == typeof(double))
                data = Convert.ToSingle(data);

            if (data.GetType() == typeof(Int64))
                data = Convert.ToInt32(data);

            return data;
        }

        #endregion
    }
}
