namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using Core.Model.NotificationTesting;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Service.Impl;

    [NapSuperAuth]
    public class NotificationTestingController : BaseController
    {
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.NotificationTesting, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.NotificationTesting, Policy = Constant.Policy.Host)]
        public ActionResult Query(QueryModel queryModel)
        {
            var notificationService = new NotificationService();

            return Json(notificationService.Query(queryModel));
        }
    }
}