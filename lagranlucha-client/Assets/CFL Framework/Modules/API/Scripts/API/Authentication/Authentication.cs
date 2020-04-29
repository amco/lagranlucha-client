using Newtonsoft.Json;

namespace CFLFramework.API
{
    public class Authentication : IAuthentication
    {
        #region FIELDS

        private const string BackendClassName = "auth_token";

        #endregion

        #region PROPERTIES

        public int Id { get; set; }
        public string Type { get; set; } = BackendClassName;
        public string Username { get; set; } = null;
        public string Email { get; set; } = null;
        public string Token { get; set; }

        [JsonProperty(propertyName: "reset_password_token")]
        public string ResetPasswordToken { get; set; }

        #endregion

        #region CONSTRUCTOR

        public Authentication() { }

        public Authentication(int id, string username, string email, string token)
        {
            Id = id;
            Username = username;
            Email = email;
            Token = token;
        }

        #endregion
    }
}
