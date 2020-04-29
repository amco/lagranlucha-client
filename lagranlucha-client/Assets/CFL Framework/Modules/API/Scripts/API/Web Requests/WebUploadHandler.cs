using System.Text;
using UnityEngine.Networking;

namespace CFLFramework.API
{
    public class WebUploadHandler : IWebUploadHandler
    {
        #region PROPERTIES

        public UploadHandler UploadHandler { get; set; }

        #endregion

        #region BEHAVIORS

        public void AddContent(string stringContent)
        {
            byte[] byteContentRaw = new UTF8Encoding().GetBytes(stringContent);
            UploadHandler = (UploadHandler)new UploadHandlerRaw(byteContentRaw);
        }

        public string GetDataString()
        {
            return Encoding.UTF8.GetString(UploadHandler.data);
        }

        #endregion
    }
}
