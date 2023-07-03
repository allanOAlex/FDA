using CsvHelper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Serilog;
using Serilog.Events;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Text;
using TB.Mvc.Session;
using TB.Shared.Constants;
using TB.Shared.Dtos;
using TB.Shared.Exceptions.Global;
using TB.Shared.Exceptions.ModelExceptions;
using TB.Shared.Requests.Auth;
using TB.Shared.Requests.Common;
using TB.Shared.Requests.Employee;
using TB.Shared.Validations.RequestValidators;

namespace TB.Mvc.Middleware
{
    public class RequestInterceptor
    {
        private readonly RequestDelegate next;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SessionDictionary<string> sessionDictionary;
        private readonly ILogger<RequestInterceptor> logger;

        public RequestInterceptor(RequestDelegate Next, IHttpContextAccessor HttpContextAccessor, SessionDictionary<string> SessionDictionary, ILogger<RequestInterceptor> Logger)
        {
            next = Next;
            httpContextAccessor = HttpContextAccessor;
            sessionDictionary = SessionDictionary;
            logger = Logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.Request.Path != "/" && 
                    context.Request.Path != "/auth/loginview" && 
                    !context.Request.Path.ToString().Contains("css") &&
                    !context.Request.Path.ToString().Contains("js") &&
                    !context.Request.Path.ToString().Contains("assets"))
                {
                    if (context.Request.Path != "/auth/login")
                    {
                        await CheckSessionValidity(context);
                    }

                    Log.Information("Incoming Request: {Method} {Path} {Time}", context.Request.Method, context.Request.Path, DateTime.Now.ToString("T", new CultureInfo("en-GB")));
                    Log.CloseAndFlush();

                    //await CheckTokenValididty(context);
                    await RemoveDuplicatesFromRequestPath(context);
                    await ValidateRequest(context);
                }
                else
                {
                    try
                    {
                        await next(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Incoming Request: {Exception} {Method} {Path}", ex, context.Request.Method, context.Request.Path);
                        await Log.CloseAndFlushAsync();
                        throw;
                    }

                       
                }
                

            }
            catch (SessionExpiredException ex)
            {
                await HandleSessionExpiredExceptionAsync(context, ex);
            }
            catch (RefreshTokenExpiredException ex)
            {
                await HandleRefreshTokenExpiredExceptionAsync(context, ex);
            }
            catch (SecurityTokenExpiredException ex)
            {
                await HandleTokenExpiredExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                throw;
            }
            finally
            {
                //Log.CloseAndFlush();
            }

        }

        public async Task<string> RemoveDuplicatesFromRequestPath(HttpContext context)
        {
            try
            {
                string inputString = context.Request.Path.Value!.ToLower();
                string result = string.Empty;

                string[] parts = inputString.Split('/');

                if (parts.Length > 1 && parts.Distinct().Count() != parts.Length)
                {
                    string[] uniqueParts = parts.Distinct().ToArray();
                    result = string.Join("/", uniqueParts);
                    context.Request.Path = new PathString(result);

                    
                }

                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CheckSessionValidity(HttpContext context)
        {
            try
            {
                var startTime = DateTime.Parse(AppConstants.SessionStartTime!);
                var sessionTimeout = context.RequestServices.GetRequiredService<IOptions<SessionOptions>>().Value.IdleTimeout.TotalSeconds;
                var elapsedTime = DateTime.Now - startTime;

                if (!AppConstants.SessionValidityChecked == true)
                {
                    if (elapsedTime.TotalSeconds >= sessionTimeout)
                    {
                        AppConstants.SessionValidityChecked = true;
                        throw new SessionExpiredException("Session has expired");

                    }
                }
                

            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task CheckTokenValididty(HttpContext context)
        {
            try
            {
                string token = context.Request.Headers["authToken"]!;
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                if (!AppConstants.TokenValidityChecked)
                {
                    bool isExpired = DateTime.UtcNow > jwtToken!.ValidTo;
                    if (isExpired)
                    {
                        AppConstants.TokenValidityChecked = true;
                        throw new SecurityTokenExpiredException("Token has expired.");
                    }
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        private async Task ValidateRequest(HttpContext httpContext)
        {
            try
            {
                var requestType = GetRequestType(httpContext);
                var contentType = httpContext.Request.ContentType?.ToLower();
                string typeName = requestType.FullName!;
                var assembly = requestType.Assembly;

                var originalRequestBodyStream = httpContext.Request.Body;

                using (var reader = new StreamReader(httpContext.Request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(requestBody))
                    {
                        using (var requestBodyStream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody)))
                        {
                            httpContext.Items["OriginalRequestBodyStream"] = originalRequestBodyStream;
                            httpContext.Request.Body = requestBodyStream;

                            Console.WriteLine($"Request Body: {requestBody}");

                            if (contentType!.Contains("application/x-www-form-urlencoded"))
                            {
                                var formData = QueryHelpers.ParseQuery(requestBody);
                                var requestInstance = Activator.CreateInstance(requestType);

                                foreach (var fieldName in formData.Keys)
                                {
                                    var propertyInfo = requestType.GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                                    if (propertyInfo != null && propertyInfo.CanWrite)
                                    {
                                        var value = Convert.ChangeType(formData[fieldName][0], propertyInfo.PropertyType);
                                        propertyInfo.SetValue(requestInstance, value);
                                    }
                                }

                                httpContext.Items["RequestInstance"] = requestInstance;
                                await ValidateRequestObject(httpContext, requestInstance!);
                            }

                            if (contentType.Contains("application/json"))
                            {
                                await ValidateRequestObject(httpContext, JsonConvert.DeserializeObject(requestBody, requestType)!);
                            }

                        }
                    }
                    else
                    {
                        AppConstants.RequestValidated = true;
                        await next(httpContext);
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        private async Task ValidateRequestObject(HttpContext context, object request)
        {
            var validator = ResolveValidator(request.GetType());
            var validationContext = new ValidationContext<object>(request);
            var validationResults = await validator.ValidateAsync(validationContext);

            if (!validationResults.IsValid)
            {
                await HandleValidationErrorsAsync(context, validationResults);
                return;
            }
            else
            {
                await next(context);
            }
        }

        private async Task HandleRequestValidation(HttpContext context)
        {
            var requestType = GetRequestType(context);

            var contentType = context.Request.ContentType?.ToLower();
            var requestBody = await GetRequestBody(context.Request);

            if (string.IsNullOrWhiteSpace(contentType))
            {
                await next(context);
                return;
            }

            if (contentType.Contains("application/json"))
            {
                var request = JsonConvert.DeserializeObject(requestBody, requestType);
                await ValidateRequestObject(context, request!);
            }
            else if (contentType.Contains("application/x-www-form-urlencoded"))
            {
                var formCollection = await context.Request.ReadFormAsync();
                var request = CreateRequestObjectFromForm(formCollection, requestType);
                await ValidateRequestObject(context, request);
            }
            else
            {
                // Unsupported content type
                context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                await context.Response.WriteAsync("Unsupported media type");
            }
        }

        private static Type GetRequestType(HttpContext context)
        {
            var path = context.Request.Path.Value!.ToLowerInvariant();

            if (path.Contains("login"))
            {
                return typeof(LoginRequest);
            }
            else if (path.Contains("logout"))
            {
                return typeof(LogoutRequest);
            }
            else if (path.Contains("forgot"))
            {
                return typeof(ForgotPasswordRequest);
            }
            else if (path.Contains("reset"))
            {
                return typeof(ResetPasswordRequest);
            }
            else if (path.Contains("unlockscreen"))
            {
                return typeof(UnlockScreenRequest);
            }
            else if (path.Contains("handletokenexpired"))
            {
                return typeof(TokenExpiredRequest);
            }
            else if (path.Contains("updateemployeesalary"))
            {
                return typeof(UpdateEmployeeSalaryRequest);
            }
            else if (path.Contains("error") || path.Contains("Error"))
            {
                return typeof(ErrorDetails);
            }
            else
            {
                return typeof(Request);
            }

        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            if (request.ContentLength == 0)
                return string.Empty;

            using var reader = new StreamReader(request.Body);
            return await reader.ReadToEndAsync();
        }

        private object CreateRequestObjectFromForm(IFormCollection formCollection, Type requestType)
        {
            var request = Activator.CreateInstance(requestType);

            foreach (var property in requestType.GetProperties())
            {
                if (formCollection.TryGetValue(property.Name, out var value))
                {
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(request, convertedValue);
                }
            }

            return request!;
        }

        private IValidator ResolveValidator(Type requestType)
        {
            if (requestType == typeof(LoginRequest))
            {
                LoginRequestValidator validator = new();
                return validator;
            }
            if (requestType == typeof(ForgotPasswordRequest))
            {
                ForgotPasswordRequestValidator validator = new();
                return validator;
            }
            if (requestType == typeof(ResetPasswordRequest))
            {
                ResetPasswordRequestValidator validator = new();
                return validator;
            }
            else if (requestType == typeof(LogoutRequest))
            {
                LogoutRequestValidator validator = new();
                return validator;
            }
            else if (requestType == typeof(UnlockScreenRequest))
            {
                UnlockScreenRequestValidator validator = new();
                return validator;
            }
            else if (requestType == typeof(ErrorDetails))
            {
                ErrorDetailsValidator validator = new();
                return validator;
            }
            else
            {
                RequestValidator validator = new();
                return validator;
            }

        }

        private async Task HandleValidationErrorsAsync(HttpContext context, FluentValidation.Results.ValidationResult validationResults)
        {
            try
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    Message = "Validation failed",
                    Errors = validationResults.Errors.Select(e => new { Property = e.PropertyName, Message = e.ErrorMessage })
                };

                var json = JsonConvert.SerializeObject(errorResponse);
                await context.Response.WriteAsync(json);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task HandleSessionExpiredExceptionAsync(HttpContext context, SessionExpiredException exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 1001;

            if (!response.HasStarted)
            {
                
                var url = $"/Error/HandleStatusCode/{response.StatusCode}";
                if (!AppConstants.HasRedirected == true)
                {
                    context.Response.Redirect(url);
                    await response.WriteAsync("Session has expired");
                    AppConstants.HasRedirected = true;
                }
                
                

            }
            else
            {
                logger.LogWarning("Can't perform redirection. Response has already started.");
            }

        }

        private async Task HandleTokenExpiredExceptionAsync(HttpContext context, SecurityTokenExpiredException exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 1002;

            if (!response.HasStarted)
            {

                var url = $"/Error/HandleStatusCode/{response.StatusCode}";
                if (!AppConstants.HasRedirected == true)
                {
                    context.Response.Redirect(url);
                    await response.WriteAsync("Token has expired");
                    AppConstants.HasRedirected = true;
                    await next(context);
                }



            }
            else
            {
                logger.LogWarning("Can't perform redirection. Response has already started.");
            }

        }

        private async Task HandleRefreshTokenExpiredExceptionAsync(HttpContext context, RefreshTokenExpiredException exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 1001;

            if (!response.HasStarted)
            {

                var url = $"/Error/HandleStatusCode/{response.StatusCode}";
                if (!AppConstants.HasRedirected == true)
                {
                    context.Response.Redirect(url);
                    await response.WriteAsync("Session has expired");
                    AppConstants.HasRedirected = true;
                    await next(context);
                }

            }
            else
            {
                logger.LogWarning("Can't perform redirection. Response has already started.");
            }

        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                string errorId = Guid.NewGuid().ToString();
                var request = context.Request;
                var response = context.Response;
                string? message = string.Empty;
                var stackTrace = string.Empty;
                
                var url = $"/Error/HandleStatusCode/";

                var exceptionType = exception.GetType();
                if (exception is FluentValidation.ValidationException validationException)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(RefreshTokenCreationExcepion))
                {
                    response.StatusCode = 1003;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(BadDataException))
                {
                    response.StatusCode = 450;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                    context.Session.SetInt32("statusCode", response.StatusCode);
                    await context.Session.CommitAsync();
                    AppConstants.StatusCode = context.Session.GetInt32("statusCode");

                }
                if (exceptionType.Name == nameof(BadRequestException))
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(ForbiddenException))
                {
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(KeyNotFoundException))
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(NotFoundException))
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(NotImplementedException))
                {
                    response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(UnauthorizedAccessException))
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(DbUpdateConcurrencyException))
                {
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(NullReferenceException))
                {
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(InvalidOperationException))
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(InvalidCastException))
                {
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                if (exceptionType.Name == nameof(IOException))
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = $"{exceptionType.Name} {exception.Message}";
                    stackTrace = exception.StackTrace;
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = exception.Message;
                    stackTrace = exception.StackTrace;
                }

                if (!response.HasStarted)
                {
                    response.ContentType = "application/json";

                    ErrorDetails errorResult = new()
                    {
                        ExceptionTypeName = exceptionType.Name,
                        Source = exception.StackTrace,
                        Message = message,
                        StatusCode = response.StatusCode,

                    };

                    ProblemDetails problemDetails = new ProblemDetails()
                    {
                        Type = exception.GetType().ToString(),
                        Title = $"Internal Server Error",
                        Status = response.StatusCode,
                        Detail = exception.StackTrace,
                        Instance = context.Request.Path,

                    };

                    if (!AppConstants.HasRedirected == true)
                    {
                        context.Response.Redirect($"{url}{response.StatusCode}");
                        await response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
                        AppConstants.HasRedirected = true;
                    }
                        
                }
                else
                {
                    logger.LogWarning("Can't write error response. Response has already started.");
                }

                
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                throw;
            }


        }

        private static void GetUrlEncodedFormData<T>(T request, out string httpRequestBody, out StreamWriter writer)
        {
            var formData = new Dictionary<string, string>();

            // Use reflection to retrieve the properties of the request object
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(request)?.ToString();
                var encodedKey = Uri.EscapeDataString(property.Name);
                var encodedValue = Uri.EscapeDataString(value ?? string.Empty);
                formData[encodedKey] = encodedValue;
            }

            httpRequestBody = string.Join("&", formData.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            // Create a new MemoryStream and write the URL-encoded form data to it
            var memoryStream = new MemoryStream();
            writer = new StreamWriter(memoryStream);
            writer.Write(httpRequestBody);
            writer.Flush();
            memoryStream.Position = 0;
        }








    }

}
