using TB.Shared.Exceptions.Global;

namespace TB.Shared.Exceptions.ModelExceptions
{
    public class SessionExpiredException : CustomException
    {
        public SessionExpiredException(string message = $"Your session has expired.") : base(message: message)
        {

        }
        
    }
}
