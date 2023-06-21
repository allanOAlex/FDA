using System.Net;

namespace TB.Shared.Exceptions.Global
{
    public class ForbiddenException : CustomException
    {
        public ForbiddenException(string message) : base(message, null, HttpStatusCode.Forbidden) { }
    }
}
