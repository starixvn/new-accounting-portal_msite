using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.MailDelivery.MailStation.Constants;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.MailDelivery.MailStation.Extensions;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.MailDelivery.MailStation.Resources;
using Fanex.MailDelivery.MailStation.Services;
using Fanex.Maintenance.Controller;
using Fanex.Maintenance.Repository.Command;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    [NapSuperAuth]
    public class MailManageController : BaseController
    {
        private readonly MailManageService mailManageService;

        public MailManageController()
        {
            mailManageService = new MailManageService();
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult Index(GetMailListCriteria criteria)
        {
            ViewBag.ListMailGroup = mailManageService.MailGroups();
            ViewBag.ListSite = mailManageService.Sites();
            ViewBag.ListStatus = mailManageService.Status();
            ViewBag.ListTriggerType = mailManageService.Trigger();
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            ViewBag.IsUseCookie = string.IsNullOrEmpty(criteria.Emails) && string.IsNullOrEmpty(criteria.MailGroupId);

            return View("~/Areas/MailStation/ManageMail/ManageMail/Index.cshtml", criteria);
        }

        public JsonResult Sites()
        {
            return Json(mailManageService.Sites());
        }

        public JsonResult MailGroups(bool hasAllElement = true)
        {
            return Json(mailManageService.MailGroups(hasAllElement));
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
        public JsonResult AddMailGroup(CreateMailGroupCriteria criteria)
        {
            return Json(mailManageService.CreateMailGroup(criteria));
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult CreateMailView(GetMailByIdCriteria criteria)
        {
            ViewBag.ListMailGroup = mailManageService.MailGroups(false);
            ViewBag.ListSite = mailManageService.Sites(false);
            ViewBag.ListTriggerType = mailManageService.Trigger(false);

            MailModel mailModel = mailManageService.MailById(criteria);

            if (mailModel != null)
            {
                mailModel.MailName += Labels.CopyPrefix;
            }

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            CreateMailCriteria model = new CreateMailCriteria(mailModel);

            return View("~/Areas/MailStation/ManageMail/CreateMail/Index.cshtml", model);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
        public JsonResult CreateMail(CreateMailCriteria criteria)
        {
            if (criteria.StartOn != null)
            {
                criteria.StartOn = criteria.StartOn.Value.ToUniversalTime();
            }

            if (criteria.EndOn != null)
            {
                criteria.EndOn = criteria.EndOn.Value.ToUniversalTime();
            }
            criteria.ModifiedBy = CurrentSession.Username;

            return Json(mailManageService.CreateMail(criteria), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult MailList(GetMailListCriteria criteria)
        {
            IList<MailModel> result = mailManageService.MailList(criteria);

            return Json(result);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult MailActionLog(int mailId)
        {
            var mailLog = mailManageService.MailLogById(new GetMailLogByIdCriteria { MailId = mailId });
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            return View("~/Areas/MailStation/ManageMail/MailActionLog/Index.cshtml", mailLog);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
        public ActionResult DeleteMail(DeleteMailCriteria criteria)
        {
            CommandResult commandResult = mailManageService.DeleteMail(criteria);

            return Json(commandResult);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult UpdateMailView(GetMailByIdCriteria criteria)
        {
            ViewBag.ListMailGroup = mailManageService.MailGroups(false);
            ViewBag.ListSite = mailManageService.Sites(false);
            ViewBag.ListTriggerType = mailManageService.Trigger(false);
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            MailModel mailModel = mailManageService.MailById(criteria);
            UpdateMailCriteria model = new UpdateMailCriteria(mailModel);

            return View("~/Areas/MailStation/ManageMail/UpdateMail/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
        public JsonResult UpdateMail(UpdateMailCriteria criteria)
        {
            criteria.ModifiedBy = CurrentSession.Username;

            CommandResult commandResult = mailManageService.UpdateMail(criteria);

            return Json(commandResult, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public JsonResult SuggestMail(SuggestMailCriteria criteria)
        {
            return Json(mailManageService.SuggestMails(criteria), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
        public void RunNow(int mailId)
        {
            mailManageService.RunNow(mailId);
        }
    }
}
