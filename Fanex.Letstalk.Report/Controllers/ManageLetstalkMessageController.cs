using Fanex.Letstalk.Notification.Criteria;
using Fanex.Letstalk.Notification.Model;
using Fanex.Letstalk.Notification.Services;
using Fanex.Maintenance.Controller;
using Fanex.Maintenance.Repository.Command;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Letstalk.Notification.Controllers
{
    [NapSuperAuth]
    public class ManageLetstalkMessageController : BaseController
    {
        private readonly ManageLetstalkMessageService manageLetstalkMessageService;

        public ManageLetstalkMessageController() : this(null)
        {
        }

        public ManageLetstalkMessageController(ILetstalkReportRestClientService service)
        {
            manageLetstalkMessageService = new LetstalkComposeServiceImp();
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.HasEdit = HasRight(SiteFunctionKeys.ManageServers, Constant.Policy.Host);
            ViewBag.StatusList = manageLetstalkMessageService.GetStatusList();
            var groupList = new List<SelectListItem> { new SelectListItem { Text = "All", Value = "", Selected = true } };
            groupList.AddRange(manageLetstalkMessageService.GetGroups());
            ViewBag.LetstalkGroupList = groupList;
            return View("~/Areas/ManageLetstalkMessage/Index.cshtml");
        }

        public ActionResult CreateLetstalkMessage()
        {
            return View("~/Areas/ManageLetstalkMessage/CreateLetstalkMessage.cshtml");
        }

        [HttpPost]
        public JsonResult InsertNotification(FormCollection formValues, IEnumerable<HttpPostedFileBase> files)
        {
            var insertNotificationCriteria = JsonConvert.DeserializeObject<InsertNotificationCriteria>(formValues["insertNotificationCriteria"]);
            insertNotificationCriteria.CreatedBy = CurrentSession.UserId;
            var result = manageLetstalkMessageService.InsertNotification(insertNotificationCriteria, files);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetGroups()
        {
            var result = manageLetstalkMessageService.GetGroups();
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetNotificationList(GetNotificationListCriteria criteria)
        {
            var result = manageLetstalkMessageService.GetNotificationList(criteria);
            return Json(result);
        }

        [HttpPost]
        public JsonResult CancelNotification(int notificationId)
        {
            var result = manageLetstalkMessageService.CancelNotificationId(notificationId);
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateNotificationStatus(UpdateNotificationStatusCriteria criteria)
        {
            var result = manageLetstalkMessageService.UpdateNotificationStatus(criteria);
            return Json(result);
        }
    }
}