using System.Web.Mvc;

namespace Fanex.Letstalk.Report
{
    public class LetstalkReportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LetstalkNotification";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_default",
                AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Letstalk.Notification.Controllers" });
        }
    }
}
