
using System.Collections.Generic;
using UnityEngine;

namespace CFLFramework.API
{
    public class RequestContent
    {
        #region FIELDS

        private const string DefaultContentType = "application/json";
        private const string ContentTypeHeader = "Content-Type";

        #endregion

        #region PROPERTIES

        public IWebRequest WebRequest { get; set; } = new WebRequest();
        public IWebUploadHandler WebUploadHandler { get; set; } = new WebUploadHandler();
        public string Endpoint { get; set; }
        public string RequestType { get; set; }
        public string Content { get; set; }
        public WWWForm Fields { get; set; }
        public Parameters Parameters { get; set; } = new Parameters();
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        #endregion

        #region CONSTRUCTOR

        public RequestContent(string requestType, string endpoint)
        {
            Headers.Add(ContentTypeHeader, DefaultContentType);
            RequestType = requestType;
            Endpoint = endpoint;
        }

        #endregion
    }
}
