using UnityEngine;
using UnityEngine.UI;
using System;

using Zenject;

using Random = UnityEngine.Random;

namespace CFLFramework.Dashboards
{
    public class DashboardExample : MonoBehaviour
    {
        #region FIELDS

        [Inject] private DashboardManager dashboardManager = null;

        [Header("COMPONENTS")]
        [SerializeField] private Dashboard dashboard = null;
        [SerializeField] private Button openButton = null;
        [SerializeField] private Button closeButton = null;
        [SerializeField] private Button randomDataButton = null;

        [Header("CONFIGURATION")]
        [SerializeField] private int daysToShow = 5;
        [SerializeField] private string tagToShow = "";

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            openButton.onClick.AddListener(OpenDashboard);
            closeButton.onClick.AddListener(CloseDashboard);
            randomDataButton.onClick.AddListener(AddRandomData);
        }

        private void OpenDashboard()
        {
            dashboard.Open(daysToShow, tagToShow);
        }

        private void CloseDashboard()
        {
            dashboard.Close();
        }

        private void AddRandomData()
        {
            float randomScore = Random.Range(1, 101);
            DateTime randomTime = DateTime.Now - new TimeSpan(Random.Range(0, 10), 0, 0, 0);
            randomTime = new DateTime(randomTime.Year, randomTime.Month, randomTime.Day);
            dashboardManager.NewDashboardData(randomScore, tagToShow, randomTime);
        }

        #endregion
    }
}
