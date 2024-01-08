namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using Data;
    using NAP.Core.Web.Attribute;
    using NAP.Core.Web.Session;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly CurrentSession _session=new CurrentSession();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _Menu()
        {
            ViewBag.Menu = _session.MenuItemList;
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }

        public ActionResult ClearFanexDataCache()
        {
            var content = CacheManager.ClearCache(100).Result;
            return Content(content);
        }
    }
}
