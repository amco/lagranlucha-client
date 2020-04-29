using System;

using Newtonsoft.Json;

namespace CFLFramework.Dashboards
{
    public class PlayData
    {
        #region FIELDS

        [NonSerialized]
        public DateTime datetime;

        #endregion

        #region PROPERTIES

        public int ID { get; set; }
        public string DateTimestamp { get; set; }
        public float Score { get; set; }
        public string Tag { get; set; }

        #endregion

        #region CONSTRUCTORS

        public PlayData() { }

        public PlayData(int id, DateTime date, float score, string tag)
        {
            ID = id;
            DateTimestamp = date.ToShortDateString();
            Score = score;
            Tag = tag;
            datetime = DateTime.Parse(DateTimestamp);
        }

        #endregion

        #region BEHAVIORS

        public string GetAsJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void LoadDate()
        {
            datetime = DateTime.Parse(DateTimestamp);
        }

        #endregion
    }
}
