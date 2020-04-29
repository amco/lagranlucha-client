using UnityEngine;
using UnityEngine.UI;

namespace CFLFramework.Dashboards
{
    public class GraphPoint : MonoBehaviour
    {
        #region FIELDS

        private const string ScoreFormat = "{0:n0}";
        private const string DateShortenformat = "{0}/{1}";
        private const char DateShortenSplitter = '/';
        private const float PointDepthOffset = 0.2f;
        private const int DayNumberIndex = 0;
        private const int MonthNumberIndex = 1;

        [Header("COMPONENTS")]
        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField] private RectTransform point = null;
        [SerializeField] private Text scoreText = null;
        [SerializeField] private Text dateText = null;

        #endregion

        #region PROPERTIES

        public Vector3 PointPosition { get => point.position; }

        #endregion

        #region BEHAVIORS

        public GraphPoint Initialize(GraphPointData data)
        {
            scoreText.text = string.Format(ScoreFormat, data.score);
            dateText.text = ShortenDateString(data.date);
            point.anchoredPosition = Vector2.up * data.yPosition;
            point.position += Vector3.back * PointDepthOffset;
            rectTransform.anchoredPosition = Vector2.right * data.xPosition;
            return this;
        }

        private string ShortenDateString(string dateString)
        {
            string[] dateStringSplit = dateString.Split(DateShortenSplitter);
            return string.Format(DateShortenformat, dateStringSplit[DayNumberIndex], dateStringSplit[MonthNumberIndex]);
        }

        #endregion
    }
}
