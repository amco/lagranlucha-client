using UnityEngine.Networking;

namespace CFLFramework.API
{
    public class WebRequest : IWebRequest
    {
        #region FIELDS

        private UnityWebRequest unityWebRequest;

        #endregion

        #region PROPERTIES

        public UnityWebRequest UnityWebRequest { get { return unityWebRequest; } }
        public bool IsNetworkError { get { return unityWebRequest.isNetworkError; } }
        public bool IsHttpError { get { return unityWebRequest.isHttpError; } }
        public long ResponseCode { get { return unityWebRequest.responseCode; } }
        public string RawResponse { get { return unityWebRequest.downloadHandler.text; } }
        public string Url { get { return unityWebRequest.url; } }
        public string Method { get { return unityWebRequest.method; } }
        public bool ChunkedTransfer { get { return unityWebRequest.chunkedTransfer; } }
        public DownloadHandler DownloadHandler { get { return unityWebRequest.downloadHandler; } }
        public UploadHandler UploadHandler { get { return unityWebRequest.uploadHandler; } }

        #endregion

        #region BEHAVIORS

        public void SetUnityWebRequest(UnityWebRequest unityWebRequest)
        {
            this.unityWebRequest = unityWebRequest;
        }

        public void SetRequestHeader(string name, string value)
        {
            unityWebRequest.SetRequestHeader(name, value);
        }

        public void SetURL(string url)
        {
            unityWebRequest.url = url;
        }

        public void SetMethod(string method)
        {
            unityWebRequest.method = method;
        }

        public void SetChunkedTransfer(bool activate)
        {
            unityWebRequest.chunkedTransfer = activate;
        }

        public void SetDownloadHandler(DownloadHandler downloadHandler)
        {
            unityWebRequest.downloadHandler = downloadHandler;
        }

        public void SetUploadHandler(UploadHandler uploadHandler)
        {
            unityWebRequest.uploadHandler = uploadHandler;
        }

        public UnityWebRequestAsyncOperation SendWebRequest()
        {
            return unityWebRequest.SendWebRequest();
        }

        #endregion
    }
}
