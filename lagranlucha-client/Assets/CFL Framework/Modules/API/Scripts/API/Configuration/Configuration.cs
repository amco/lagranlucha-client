using UnityEngine;

namespace CFLFramework.API
{
    [CreateAssetMenu(menuName = MenuName)]
    public class Configuration : ScriptableObject
    {
        #region FIELDS

        private const string MenuName = "CFL Framework/API/Create Configuration";

        [Header("CONFIGURATIONS")]
        [SerializeField] [TextArea] private string host = "https://10.100.11.237:4000/";
        [SerializeField] [TextArea] private string secretToken = "";

        #endregion

        #region PROPERTIES

        public string Host { get => host; }
        public string SecretToken { get => secretToken; }

        #endregion

        #region BEHAVIORS

        public void Initialize(string host, string secretToken)
        {
            this.host = host;
            this.secretToken = secretToken;
        }

        #endregion
    }
}
