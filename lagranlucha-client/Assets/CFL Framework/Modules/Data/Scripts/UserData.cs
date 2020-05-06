using System.Collections.Generic;

using Newtonsoft.Json;
using CFLFramework.Utilities.Extensions;

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
            var userData = Data;
            userData.SetValue(keys, data);
        }

        internal T GetData<T>(string[] keys, object defaultValue)
        {
            var userData = Data;
            return userData.GetValue<T>(keys, defaultValue);
        }

        internal List<T> GetDataList<T>(string[] keys, object defaultValue)
        {
            var userData = Data;
            return userData.GetValueList<T>(keys, defaultValue);
        }

        internal void MergeData(Dictionary<string, object> newDictionary)
        {
            var userData = Data;
            userData.MergeValue(newDictionary);
        }

        internal bool HasData(string[] keys)
        {
            var userData = Data;
            return userData.HasValue(keys);
        }

        #endregion
    }
}
