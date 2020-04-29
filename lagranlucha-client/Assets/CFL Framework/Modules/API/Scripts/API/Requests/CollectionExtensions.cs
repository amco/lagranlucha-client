using System.Text;
using System.Collections.Generic;

namespace CFLFramework.API
{
    public static class CollectionExtensions
    {
        #region FIELDS

        private const string ContinuosQueryFormat = "{0}={1}&";
        private const string EndQueryFormat = "{0}={1}";

        #endregion

        #region BEHAVIORS

        public static string ToQueryString(this IDictionary<string, string> urlDictionary)
        {
            if (urlDictionary.Count == 0)
                return string.Empty;

            StringBuilder buffer = new StringBuilder();
            int count = 0;
            bool isEndString = false;

            foreach (var key in urlDictionary.Keys)
            {
                if (count == urlDictionary.Count - 1)
                    isEndString = true;

                buffer.AppendFormat(isEndString ? EndQueryFormat : ContinuosQueryFormat, key, urlDictionary[key]);
                count++;
            }

            return buffer.ToString();
        }

        #endregion
    }
}
