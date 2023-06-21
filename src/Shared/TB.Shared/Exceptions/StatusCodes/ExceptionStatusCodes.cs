using System.Net;
using TB.Shared.Exceptions.ModelExceptions;

namespace TB.Shared.Exceptions.StatusCodes
{
    public static class ExceptionStatusCodes
    {
        private static Dictionary<Type, HttpStatusCode> statusCodes = new()
        {
            {typeof(SessionExpiredException), HttpStatusCode.Redirect }
        };

        public static HttpStatusCode GetHttpStatusCode(this Exception exception)
        {
            bool found = statusCodes.TryGetValue(exception.GetType(), out HttpStatusCode statusCode);   
            return found ? statusCode : HttpStatusCode.InternalServerError;
        }


    }
}
