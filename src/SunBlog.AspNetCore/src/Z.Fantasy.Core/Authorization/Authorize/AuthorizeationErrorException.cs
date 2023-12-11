namespace Z.Fantasy.Core.Authorization.Authorize
{
    public class AuthorizeationErrorException : Exception
    {
        public AuthorizeationErrorException(string message) : base(message) { }

        public AuthorizeationErrorException() { }

        public AuthorizeationErrorException(string message, Exception innerException) : base(message, innerException) { }
    }

    public static class ThrowAuthorizeationError
    {
        public static void ThrowAuthorizeationErro(string message)
        {
            throw new AuthorizeationErrorException(message);
        }
    }
}
