using System.Collections.Generic;
using System.Text;

namespace CFLFramework.API
{
    public class Parameters
    {
        #region FIELDS

        private const string ContinuosQueryFormat = "{0}={1}&";
        private const string EndQueryFormat = "{0}={1}";

        private List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

        #endregion

        #region BEHAVIORS

        public void Add(string key, string value)
        {
            parameters.Add(new KeyValuePair<string, string>(key, value));
        }

        public string ToQueryString()
        {
            if (parameters.Count == 0)
                return string.Empty;

            StringBuilder buffer = new StringBuilder();
            int count = 0;
            bool isEndString = false;

            foreach (var pair in parameters)
            {
                if (count == parameters.Count - 1)
                    isEndString = true;

                buffer.AppendFormat(isEndString ? EndQueryFormat : ContinuosQueryFormat, pair.Key, pair.Value);
                count++;
            }

            return buffer.ToString();
        }

        #endregion
    }
}
