namespace Fanex.Maintenance.Site.Areas.MailStation
{
    using System.Web.Mvc;

    public class MailStationAreaRegistration : AreaRegistration
    {
        public override string AreaName => "MailStation";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_default",
                AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.MailDelivery.MailStation.Controllers" });
        }
    }
}
