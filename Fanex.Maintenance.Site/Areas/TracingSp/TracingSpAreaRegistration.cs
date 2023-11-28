namespace Fanex.Maintenance.Site.Areas.TracingSp
{
    using System.Web.Mvc;

    public class TracingSpAreaRegistration : AreaRegistration
    {
        public override string AreaName => "ProfilerSP";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_DefaultAreas",
                AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Tool.TraceSP.Controllers" });            
        }
    }
}
