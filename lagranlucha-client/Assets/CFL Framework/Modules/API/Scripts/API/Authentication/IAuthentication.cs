namespace CFLFramework.API
{
    public interface IAuthentication
    {
        #region FIELDS

        int Id { get; set; }
        string Type { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Token { get; set; }
        string ResetPasswordToken { get; set; }

        #endregion
    }
}
