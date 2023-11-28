using Fanex.Logging;
using System;
using System.Web;

namespace Fanex.MailDelivery.MailStation.Services
{
    public static class LoggingHandler
    {
        public static void Log(Exception ex)
        {
            var message = ex.Message +
                          "<br>\r\nSOURCE: " + ex.Source +
                          "<br>\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                          "<br>\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                          "<br>\r\nFORM: " + HttpContext.Current.Request.Form +
                          "<br>\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString +
                          "<br>\r\nTARGETSITE: " + ex.TargetSite +
                          "<br>\r\nSTACKTRACE: " + ex.StackTrace;

            Logger.Log.Error(message);
        }
    }
}
