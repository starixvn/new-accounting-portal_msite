using System.Web.Mvc;

namespace Fanex.DeleteTempVariable
{
    public class DeleteTempVariableAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TempVariable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName + "_default",
                AreaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.DeleteTempVariable.Controllers" });
        }
    }
}