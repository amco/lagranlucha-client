using UnityEngine.Networking;

namespace CFLFramework.API
{
    public interface IWebRequest
    {
        #region PROPERTIES

        UnityWebRequest UnityWebRequest { get; }
        bool IsHttpError { get; }
        bool IsNetworkError { get; }
        long ResponseCode { get; }
        string RawResponse { get; }
        string Url { get; }
        string Method { get; }
        bool ChunkedTransfer { get; }
        DownloadHandler DownloadHandler { get; }
        UploadHandler UploadHandler { get; }

        #endregion

        #region BEHAVIORS

        void SetUnityWebRequest(UnityWebRequest unityWebRequest);
        void SetRequestHeader(string name, string value);
        void SetURL(string url);
        void SetMethod(string method);
        void SetChunkedTransfer(bool activate);
        void SetDownloadHandler(DownloadHandler downloadHandler);
        void SetUploadHandler(UploadHandler uploadHandler);
        UnityWebRequestAsyncOperation SendWebRequest();

        #endregion
    }
}
