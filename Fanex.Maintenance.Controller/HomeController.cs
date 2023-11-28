namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using NAP.Core.Web.Attribute;

    [NapSuperAuth]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
