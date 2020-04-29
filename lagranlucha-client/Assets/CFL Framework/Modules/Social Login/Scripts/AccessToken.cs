using Newtonsoft.Json;

namespace CFLFramework.SocialLogin
{
    public class AccessToken
    {
        #region FIELDS

        private const string IdKey = "id";
        private const string TypeKey = "type";
        private const string AccessTokenKey = "access_token";
        private const string TypeName = "omniauth_token";

        #endregion

        #region PROPERTIES

        [JsonProperty(IdKey)] public string Id { get; private set; } = null;
        [JsonProperty(TypeKey)] public string Type { get; private set; } = TypeName;
        [JsonProperty(AccessTokenKey)] public string Token { get; private set; } = null;

        #endregion

        #region CONSTRUCTORS

        public AccessToken() : this(null) { }

        [JsonConstructor]
        public AccessToken(string token)
        {
            Token = token;
        }

        #endregion
    }
}
