namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Service.Model.LetstalkNotification;
    using Service.Testing;

    [NapSuperAuth]
    public class TestingController : BaseController
    {
        private readonly INotificationService _notificationService;

        public TestingController() :
            this(null)
        {
        }

        public TestingController(INotificationService notificationService)
        {
            _notificationService = notificationService ?? new NotificationService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.Risk_ControlLetstalkNotification, Policy = Constant.Policy.Read)]
        public ActionResult LetstalkNotification()
        {
            return View();
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.Risk_ControlLetstalkNotification, Policy = Constant.Policy.Read)]
        public ActionResult LetstalkNotification_Query(QueryModel queryModel)
        {
            return Json(_notificationService.Query(queryModel));
        }
    }
}
