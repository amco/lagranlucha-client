using System;

using Newtonsoft.Json;

namespace LaGranLucha.API
{
    public class Branch
    {
        #region FIELDS

        private const string OpenAtBackend = "open_at";
        private const string CloseAtBackend = "close_at";

        #endregion

        #region PROPERTIES

        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        [JsonProperty(OpenAtBackend)] public TimeSpan OpenAt { get; set; }
        [JsonProperty(CloseAtBackend)] public TimeSpan CloseAt { get; set; }

        #endregion
    }
}
