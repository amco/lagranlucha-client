using UnityEngine;
using System.Collections.Generic;

namespace CFLFramework.Dashboards
{
    public class DashboardGraph : MonoBehaviour
    {
        #region FIELDS

        private const int ExtraColumns = 2;

        [Header("PREFABS")]
        [SerializeField] private GraphPoint graphPointPrefab = null;

        [Header("COMPONENTS")]
        [SerializeField] private RectTransform containerRectTransform = null;

        [Header("CONFIGURATIONS")]
        [SerializeField] private float extraRangePercentage = 0.2f;

        private List<GraphPoint> graphPoints = new List<GraphPoint>();

        #endregion

        #region PROPERTIES

        private float Height { get => containerRectTransform.rect.height; }
        private float Width { get => containerRectTransform.rect.width; }
        private float MinValue { get; set; }
        private float MaxValue { get; set; }
        private float ColumnWidth { get; set; }

        #endregion

        #region BEHAVIORS

        public void Initialize(float minValue, float maxValue, int daysToShow)
        {
            MinValue = minValue * (1f - extraRangePercentage);
            MaxValue = maxValue * (1f + extraRangePercentage);
            ColumnWidth = Width / (daysToShow + ExtraColumns);
        }

        public void ShowElements(GraphPointData[] graphData)
        {
            ResetGraph();
            for (int i = 0; i < graphData.Length; i++)
            {
                graphData[i].SetPosition(ColumnWidth * (i + 1), GetPointHeight(graphData[i].score));
                graphPoints.Add(CreateGraphPoint(graphData[i]));
            }
        }

        private float GetPointHeight(float score)
        {
            float t = Mathf.InverseLerp(MinValue, MaxValue, score);
            return Mathf.Lerp(0, Height, t);
        }

        private GraphPoint CreateGraphPoint(GraphPointData data)
        {
            return Instantiate(graphPointPrefab, containerRectTransform).Initialize(data);
        }

        public void ResetGraph()
        {
            foreach (GraphPoint graphPoint in graphPoints)
                Destroy(graphPoint.gameObject);

            graphPoints.Clear();
        }

        #endregion
    }
}
