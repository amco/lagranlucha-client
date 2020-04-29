using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

using Zenject;
using Newtonsoft.Json;
using CFLFramework.Time;

namespace CFLFramework.Dashboards
{
    public class DashboardManager : MonoBehaviour
    {
        #region FIELDS

        private const string ResetToolName = "CFL Framework/Dashboards/Reset Dashboards Data";
        private const string AnalyticsDataJsonsKey = "AnalyticsData";
        private const string CurrentIDKey = "CurrentID";
        private const int DefaultID = 0;

        [Inject] private TimeManager timeManager = null;

        private List<PlayData> analyticsData = null;
        private int currentID = 0;

        #endregion

        #region PROPERTIES

        public PlayData[] AnalyticsData { get => analyticsData.ToArray(); }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            GetAnalyticsDataJsons();
        }

        private void GetAnalyticsDataJsons()
        {
            string[] analyticsDataJsons = PlayerPrefsExtension.GetStringArray(AnalyticsDataJsonsKey);
            currentID = PlayerPrefs.GetInt(CurrentIDKey, DefaultID);

            analyticsData = new List<PlayData>();
            foreach (string dataJson in analyticsDataJsons)
            {
                PlayData newData = JsonConvert.DeserializeObject<PlayData>(dataJson);
                newData.LoadDate();
                analyticsData.Add(newData);
            }
        }

        public void NewDashboardData(float score, string tag)
        {
            NewDashboardData(score, tag, timeManager.GetDate());
        }

        public void NewDashboardData(float score, string tag, DateTime dateTime)
        {
            analyticsData.Add(new PlayData(currentID, dateTime, score, tag));
            currentID++;
            SaveDashboardData();
        }

        private void SaveDashboardData()
        {
            PlayerPrefs.SetInt(CurrentIDKey, currentID);

            var jsonData = new List<string>();
            foreach (PlayData data in analyticsData)
                jsonData.Add(data.GetAsJson());

            PlayerPrefsExtension.SetStringArray(AnalyticsDataJsonsKey, jsonData.ToArray());
        }

        public List<List<PlayData>> FilterData(int days, string tag)
        {
            List<List<PlayData>> filteredData = new List<List<PlayData>>();

            if (analyticsData.Count == 0)
                return filteredData;

            for (int i = 0; i < days; i++)
                filteredData.Add(new List<PlayData>());

            PlayData[] data = analyticsData.ToArray();
            data = data.OrderByDescending(x => x.datetime).ToArray();
            for (int i = 0, j = 0; i < data.Length; i++)
            {
                if (data[i].Tag != tag)
                    continue;

                if (!IsListEmpty(filteredData[j]) && DifferentDate(filteredData[j].First(), data[i]) && ++j == days)
                    break;

                filteredData[j].Add(data[i]);
            }

            filteredData.RemoveAll(list => list.Count == 0);
            return filteredData;
        }

        private bool DifferentDate(PlayData one, PlayData two)
        {
            return one.datetime != two.datetime;
        }

        private bool IsListEmpty(List<PlayData> list)
        {
            return list.Count == 0;
        }

#if UNITY_EDITOR
        [MenuItem(ResetToolName)]
#endif
        private static void ResetData()
        {
            PlayerPrefsExtension.SetStringArray(AnalyticsDataJsonsKey, new string[] { });
            PlayerPrefs.SetInt(CurrentIDKey, 0);
        }

        #endregion
    }
}
