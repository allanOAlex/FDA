using TB.Shared.Exceptions.Global;

namespace TB.Shared.Exceptions.ModelExceptions
{
    public class RefreshTokenExpiredException : CustomException
    {
        public RefreshTokenExpiredException(string message = "Refresh token has expired") : base(message) { }   

    }
}
