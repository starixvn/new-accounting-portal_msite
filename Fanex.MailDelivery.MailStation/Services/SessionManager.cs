namespace Fanex.MailDelivery.MailStation.Services
{
    using System;
    using System.Web;

    public static class SessionManager
    {
        public static void Set<T>(string sessionKey, T value)
        {
            try
            {
                HttpContext.Current.Session[sessionKey] = value;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
        }

        public static T Get<T>(string sessionKey)
        {
            try
            {
                var value = (T)HttpContext.Current.Session[sessionKey];
                return value;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static void Set(string sessionKey, object value)
        {
            SessionManager.Set<object>(sessionKey, value);
        }

        public static object Get(string sessionKey)
        {
            return SessionManager.Get<object>(sessionKey);
        }
    }
}
