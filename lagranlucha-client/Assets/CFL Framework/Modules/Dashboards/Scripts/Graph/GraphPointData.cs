using UnityEngine;

namespace CFLFramework.Dashboards
{
    public class GraphPointData
    {
        #region FIELDS

        public float score;
        public string date;
        public float xPosition;
        public float yPosition;

        #endregion

        #region CONSTRUCTOR

        public GraphPointData(float score, string date)
        {
            this.score = score;
            this.date = date;
        }

        #endregion

        #region BEHAVIORS

        public void SetPosition(float xPosition, float yPosition)
        {
            this.xPosition = xPosition;
            this.yPosition = yPosition;
        }

        #endregion
    }
}
