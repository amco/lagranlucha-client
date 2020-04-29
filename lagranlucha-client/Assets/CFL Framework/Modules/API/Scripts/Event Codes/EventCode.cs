using Newtonsoft.Json;

namespace CFLFramework.EventCodes
{
    public class EventCode
    {
        #region FIELDS

        private const string StartDateKey = "start_date";
        private const string EndDateKey = "end_date";

        #endregion

        #region PROPERTIES

        public int Id { get; set; }
        public string Token { get; set; }
        public string Event { get; set; }
        [JsonProperty(propertyName: StartDateKey)] public string StartDate { get; set; }
        [JsonProperty(propertyName: EndDateKey)] public string EndDate { get; set; }
        public string Status { get; set; }

        #endregion
    }
}
