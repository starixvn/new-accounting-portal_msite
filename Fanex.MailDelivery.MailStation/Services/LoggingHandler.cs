namespace Fanex.MailDelivery.MailStation.Services
{
    using System;
    using System.Web;
    using Fanex.Logging;

    public enum LogType : int
    {
        /// <summary>
        /// The information type.
        /// </summary>
        Info = 1,

        /// <summary>
        /// The error type.
        /// </summary>
        Error = 2
    }

    public class LoggingHandler
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void Log(Exception ex)
        {
            string message = ex.Message +
                              "<br>\r\nSOURCE: " + ex.Source +
                              "<br>\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                              "<br>\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                              "<br>\r\nFORM: " + HttpContext.Current.Request.Form +
                              "<br>\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString +
                              "<br>\r\nTARGETSITE: " + ex.TargetSite +
                              "<br>\r\nSTACKTRACE: " + ex.StackTrace;
            Logger.Log.Error(message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            if (logType == LogType.Info)
            {
                LogService.Info(message);
            }
            else if (logType == LogType.Error)
            {
                LogService.Error(message);
            }
        }
    }
}