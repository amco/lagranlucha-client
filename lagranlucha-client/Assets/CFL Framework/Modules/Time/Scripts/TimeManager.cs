using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace CFLFramework.Time
{
    public class TimeManager : MonoBehaviour
    {
        #region FIELDS

        private const string WorldClockURL = "http://timestampnow.com/";

        [Header("CONFIGURATIONS")]
        [SerializeField] public ClockFormat clockFormat;

        private IDateTimeService dateTimeService;
        private DateTime currentWorldDateTime;

        #endregion

        #region EVENTS

        public event UnityAction onTimeLoaded;

        #endregion

        #region PROPERTIES

        public IDateTimeService DateTimeService
        {
            get
            {
                if (dateTimeService == null)
                    return dateTimeService = new DateTimeService();

                return dateTimeService;
            }
            set
            {
                dateTimeService = value;
            }
        }

        public ClockFormat ClockFormat { set => clockFormat = value; }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            UpdateTime();
        }

        public void UpdateTime()
        {
            StartCoroutine(UpdateWorldTime());
        }

        public DateTime GetDate()
        {
            DateTime date = GetTime();
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public DateTime GetTime()
        {
            switch (clockFormat)
            {
                case ClockFormat.World:
                    return GetWorldTime();
                default:
                    return GetLocalTime();
            }
        }

        public string GetTimeAsString(string format)
        {
            return GetTime().ToString(format);
        }

        private IEnumerator UpdateWorldTime()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(WorldClockURL);
            yield return webRequest.SendWebRequest();

            currentWorldDateTime = webRequest.isNetworkError ? GetLocalTime() : ConvertFromUnixTimestamp(webRequest);
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
            currentWorldDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentWorldDateTime, timeInfo);
            onTimeLoaded?.Invoke();
        }

        private DateTime GetLocalTime()
        {
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
            DateTime currentLocalDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeService.UtcNow, timeInfo);
            return currentLocalDateTime;
        }

        private DateTime GetWorldTime()
        {
            return currentWorldDateTime;
        }

        private DateTime ConvertFromUnixTimestamp(UnityWebRequest request)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(ParseDoubleFromRequest(request.downloadHandler.text));
        }

        private double ParseDoubleFromRequest(string stringTimestamp)
        {
            return double.Parse(stringTimestamp);
        }

        public long GetTimeTicks()
        {
            return GetTime().Ticks;
        }

        public double GetTimeMilliseconds()
        {
            return TimeSpan.FromTicks(GetTimeTicks()).TotalMilliseconds;
        }

        #endregion
    }
}
