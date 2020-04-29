using System.Collections.Generic;

using Newtonsoft.Json;

namespace CFLFramework.API
{
    public partial class Errors
    {
        #region PROPERTIES

        [JsonProperty(propertyName: "errors")]
        public ErrorDetail[] Details { get; set; }
        public string Error { get; set; }

        #endregion

        #region CONSTRUCTORS

        public Errors(string errorMessage = "")
        {
            Error = errorMessage;
        }

        #endregion

        #region BEHAVIORS

        public List<string> Messages()
        {
            List<string> errorMessages = new List<string>();
            if (!string.IsNullOrEmpty(Error))
                errorMessages.Add(Error);

            if (Details != null)
                foreach (ErrorDetail error in Details)
                    errorMessages.Add(error.Detail);

            return errorMessages;
        }

        #endregion
    }
}
