using UnityEngine.Networking;

namespace CFLFramework.API
{
    public interface IWebUploadHandler
    {
        #region PROPERTIES

        UploadHandler UploadHandler { get; set; }

        #endregion

        #region BEHAVIORS

        void AddContent(string stringContent);
        string GetDataString();

        #endregion
    }
}
