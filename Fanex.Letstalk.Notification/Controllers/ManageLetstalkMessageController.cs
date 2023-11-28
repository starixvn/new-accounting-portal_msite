using Fanex.Letstalk.Notification.Criteria;
using Fanex.Letstalk.Notification.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
            manageLetstalkMessageService = new ManageLetstalkMessageServiceImp();
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.HasEdit = HasRight(SiteFunctionKeys.ManageLetstalkMessage, Constant.Policy.Read);
            ViewBag.StatusList = manageLetstalkMessageService.GetStatusList();
            ViewBag.LetstalkGroupList = manageLetstalkMessageService.GetGroups();
            return View("~/Areas/ManageLetstalkMessage/Index.cshtml");
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        public ActionResult CreateLetstalkMessage()
        {
            ViewBag.LetstalkGroupList = manageLetstalkMessageService.GetGroups();
            return View("~/Areas/ManageLetstalkMessage/CreateLetstalkMessage.cshtml");
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        public ActionResult EditLetstalkMessage(int notificationID, int recipientID)
        {
            var criteria = new GetNotificationListCriteria
            {
                RecipientID = recipientID,
                NotificationId = notificationID
            };
            ViewBag.LetstalkGroupList = manageLetstalkMessageService.GetGroups();
            var model = manageLetstalkMessageService.GetNotificationById(criteria).FirstOrDefault();
            return View("~/Areas/ManageLetstalkMessage/EditLetstalkMessage.cshtml", model);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult InsertNotification(FormCollection formValues, IEnumerable<HttpPostedFileBase> files)
        {
            var insertNotificationCriteria = JsonConvert.DeserializeObject<InsertNotificationCriteria>(formValues["insertNotificationCriteria"]);
            insertNotificationCriteria.CreatedBy = CurrentSession.UserId;
            var result = manageLetstalkMessageService.InsertNotification(insertNotificationCriteria, files);
            return Json(result);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult EditNotification(FormCollection formValues, IEnumerable<HttpPostedFileBase> files)
        {
            var editNotificationCriteria = JsonConvert.DeserializeObject<EditNotificationCriteria>(formValues["editNotificationCriteria"]);
            editNotificationCriteria.LastModifiedBy = CurrentSession.UserId;
            var result = manageLetstalkMessageService.EditNotification(editNotificationCriteria, files);
            return Json(result);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult DeleteNotification(DeleteNotificationCriteria criteria)
        {
            criteria.LastModifiedBy = CurrentSession.UserId;
            var result = manageLetstalkMessageService.DeleteNotification(criteria);
            return Json(result);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult ResendNotification(ResendNotificationCriteria criteria)
        {
            var result = manageLetstalkMessageService.ResendNotification(criteria);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetGroups()
        {
            var result = manageLetstalkMessageService.GetGroups();
            return Json(result);
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ManageLetstalkMessage, Policy = Constant.Policy.Read)]
        [HttpPost]
        public JsonResult GetNotificationList(GetNotificationListCriteria criteria)
        {
            var result = manageLetstalkMessageService.GetNotificationList(criteria);
            return Json(result);
        }
    }
}