using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Zenject;
using CFLFramework.Warnings;
using CFLFramework.Utilities.Inspector;

namespace CFLFramework.Dashboards
{
    public class Dashboard : MonoBehaviour
    {
        #region FIELDS

        private const string EmptyDashboardMessage = "The dashboard is empty.";
        private const int MinimumDaysToShow = 2;
        private const int MaximumDaysToShow = 10;

        [Inject] protected DashboardManager dashboardManager = null;
        [Inject] private WarningsManager warningsManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private DashboardGraph dashboardGraph = null;
        [SerializeField] private GameObject container = null;

        [Header("STATES")]
        [ReadOnly] [SerializeField] protected int daysToShow = 0;
        [ReadOnly] [SerializeField] protected new string tag = "";

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            Close();
        }

        public void Open(int daysToShow, string tag)
        {
            try
            {
                this.daysToShow = Mathf.Clamp(daysToShow, MinimumDaysToShow, MaximumDaysToShow);
                this.tag = tag;

                if (string.IsNullOrEmpty(tag))
                    throw new NoDataException();

                GraphPointData[] graphData = CreateGraphData();
                Appear();
                dashboardGraph.Initialize(GetMinScore(graphData), GetMaxScore(graphData), this.daysToShow);
                dashboardGraph.ShowElements(graphData);

            }
            catch (NoDataException)
            {
                warningsManager.ShowWarning(EmptyDashboardMessage);
            }
        }

        private void Appear()
        {
            container.SetActive(true);
        }

        public void Close()
        {
            container.SetActive(false);
            ResetDashboard();
        }

        private void ResetDashboard()
        {
            dashboardGraph.ResetGraph();
        }

        private float GetMaxScore(GraphPointData[] data)
        {
            return data.Max(d => d.score);
        }

        private float GetMinScore(GraphPointData[] data)
        {
            return data.Min(d => d.score);
        }

        private GraphPointData[] CreateGraphData()
        {
            List<List<PlayData>> filteredData = dashboardManager.FilterData(daysToShow, tag);
            if (filteredData.Count == 0)
                throw new NoDataException();

            List<GraphPointData> graphData = new List<GraphPointData>();
            for (int i = 0; i < filteredData.Count; i++)
            {
                float sum = 0;
                for (int j = 0; j < filteredData[i].Count; j++)
                    sum += filteredData[i][j].Score;

                float average = sum / filteredData[i].Count;
                graphData.Add(new GraphPointData(average, filteredData[i].First().DateTimestamp));
            }

            graphData.Reverse();
            return graphData.ToArray();
        }

        #endregion
    }
}
