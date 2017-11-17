namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Home Controller.
    /// </summary>
    [NapSuperAuth]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index Page.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}