using Fanex.Letstalk.Notification.Criteria;
using Fanex.Letstalk.Notification.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Command;
using System.Collections.Generic;
using System.Web.Mvc;

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
        public JsonResult InsertNotification(InsertNotificationCriteria criteria)
        {
            //var result = letstalkComposeService.InsertNotification(criteria);
            var result = new CommandResult { Code = 0, Message = "" };
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