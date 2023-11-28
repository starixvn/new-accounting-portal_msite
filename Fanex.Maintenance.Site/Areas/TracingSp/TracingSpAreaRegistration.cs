using System.Web.Mvc;

namespace Fanex.Maintenance.Site
{
    public class TracingSpAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ProfilerSP";
            }
        }

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