namespace CFLFramework.API
{
    public enum HttpStatusCode
    {
        Undefined = -1,
        CouldNotConnectToHost = 0,
        Ok = 200,
        Created = 201,
        NoContent = 204,
        NotModified = 304,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        UnprocessableEntity = 422,
        InvalidJSON = 600
    }
}
