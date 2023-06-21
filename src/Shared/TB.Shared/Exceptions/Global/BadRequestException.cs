using System.Net;

namespace TB.Shared.Exceptions.Global
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(message, null, HttpStatusCode.BadRequest) { }
    }
}
