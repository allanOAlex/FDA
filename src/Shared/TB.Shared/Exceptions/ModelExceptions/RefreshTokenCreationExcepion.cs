using TB.Shared.Exceptions.Global;

namespace TB.Shared.Exceptions.ModelExceptions
{
    public class RefreshTokenCreationExcepion : CustomException
    {
        public RefreshTokenCreationExcepion(string message = $"Could not create Refresh token.") : base(message: message)
        {

        }
    }
}
