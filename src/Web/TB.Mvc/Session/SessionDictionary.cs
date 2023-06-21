using System.Text.Json;

namespace TB.Mvc.Session
{
    public class SessionDictionary<T>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string cookieName;

        public SessionDictionary(IHttpContextAccessor HttpContextAccessor, string CookieName)
        {
            httpContextAccessor = HttpContextAccessor;
            cookieName = CookieName;
        }

        private IDictionary<string, T>? GetSessionDictionary()
        {
            var session = httpContextAccessor.HttpContext!.Session;
            var sessionJson = session.GetString(cookieName);
            return sessionJson != null ? JsonSerializer.Deserialize<IDictionary<string, T>>(sessionJson) : new Dictionary<string, T>();
        }

        private void SetSessionDictionary(IDictionary<string, T> dictionary)
        {
            var session = httpContextAccessor.HttpContext!.Session;
            var sessionJson = JsonSerializer.Serialize(dictionary);
            session.SetString(cookieName, sessionJson);
        }

        public T this[string key]
        {
            get
            {
                var dictionary = GetSessionDictionary();
                return dictionary!.ContainsKey(key) ? dictionary[key] : default(T)!;
            }
            set
            {
                var dictionary = GetSessionDictionary();
                dictionary![key] = value;
                SetSessionDictionary(dictionary);
            }
        }
    }

}
