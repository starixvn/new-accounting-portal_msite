namespace Fanex.Letstalk.Notification
{
    using System.Web.Mvc;

    public class LetstalkReportAreaRegistration : AreaRegistration
    {
        public override string AreaName => "LetstalkNotification";

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
