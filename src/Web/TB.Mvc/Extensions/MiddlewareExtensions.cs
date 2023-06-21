using TB.Mvc.Middleware;

namespace TB.Mvc.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureGlobalExceptionHandler(this IApplicationBuilder app)
        {

            app.UseMiddleware<RequestInterceptor>();
        }
    }
}
