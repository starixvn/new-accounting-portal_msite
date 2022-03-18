using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Fanex.Letstalk.Notification.Criteria;
using Fanex.Letstalk.Notification.Services;
using Fanex.Maintenance.Controller;
using Newtonsoft.Json;

namespace Fanex.Letstalk.Notification.Controllers
{
    public class LetstalkComposeController : BaseController
    {
        private readonly LetstalkComposeService letstalkComposeService;

        public LetstalkComposeController() : this(null)
        {
        }

        public LetstalkComposeController(ILetstalkReportRestClientService service)
        {
            letstalkComposeService = new LetstalkComposeServiceImp();
        }

        public ActionResult Index()
        {
            return View("~/Areas/LetstalkCompose/Index.cshtml");
        }

        [HttpPost]
        public JsonResult InsertNotification(FormCollection formValues, IEnumerable<HttpPostedFileBase> files)
        {
            var insertNotificationCriteria = JsonConvert.DeserializeObject<InsertNotificationCriteria>(formValues["insertNotificationCriteria"]);
            var result = letstalkComposeService.InsertNotification(insertNotificationCriteria, files);
            //var result = new CommandResult { Code = 0, Message = "" };
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetGroups()
        {
            var result = letstalkComposeService.GetGroups();
            return Json(result);
        }
    }
}