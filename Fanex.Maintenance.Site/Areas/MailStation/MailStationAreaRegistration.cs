namespace Fanex.Maintenance.Site
{
    using System.Web.Mvc;

    /// <summary>
    /// User Login Area.
    /// </summary>
    public class MailStationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MailStation";
            }
        }

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