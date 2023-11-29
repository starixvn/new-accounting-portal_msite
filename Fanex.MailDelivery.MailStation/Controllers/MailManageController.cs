using System.Web.Mvc;
using Fanex.MailDelivery.MailStation.Constants;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.MailDelivery.MailStation.Extensions;
using Fanex.MailDelivery.MailStation.Resources;
using Fanex.MailDelivery.MailStation.Services;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    using System.Linq;
    using NAP.Core.Web.Session;

    [NapSuperAuth]
    public class MailManageController : Controller
    {
        private readonly MailManageService mailManageService = new MailManageService();

        private readonly CurrentSession currentSession = new CurrentSession();

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

            var mailModel = mailManageService.MailById(criteria);

            if (mailModel != null)
            {
                mailModel.MailName += Labels.CopyPrefix;
            }

            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();
            var model = new CreateMailCriteria(mailModel);

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
            criteria.ModifiedBy = currentSession.Username;

            return Json(mailManageService.CreateMail(criteria), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult MailList(GetMailListCriteria criteria)
        {
            var result = mailManageService.MailList(criteria);

            return Json(result);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult MailActionLog(int mailId)
        {
            var mailLog = mailManageService.MailLogById(new GetMailLogByIdCriteria { MailId = mailId });
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            return View("~/Areas/MailStation/ManageMail/MailActionLog/Index.cshtml", mailLog.ToList());
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
        public ActionResult DeleteMail(DeleteMailCriteria criteria)
        {
            var commandResult = mailManageService.DeleteMail(criteria);

            return Json(commandResult);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Read)]
        public ActionResult UpdateMailView(GetMailByIdCriteria criteria)
        {
            ViewBag.ListMailGroup = mailManageService.MailGroups(false);
            ViewBag.ListSite = mailManageService.Sites(false);
            ViewBag.ListTriggerType = mailManageService.Trigger(false);
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            var mailModel = mailManageService.MailById(criteria);
            var model = new UpdateMailCriteria(mailModel);

            return View("~/Areas/MailStation/ManageMail/UpdateMail/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.ManageMail, Policy = Constant.Policy.Host)]
        public JsonResult UpdateMail(UpdateMailCriteria criteria)
        {
            criteria.ModifiedBy = currentSession.Username;

            var commandResult = mailManageService.UpdateMail(criteria);

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
