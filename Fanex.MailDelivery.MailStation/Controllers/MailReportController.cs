using Fanex.MailDelivery.MailStation.Constants;
using Fanex.MailDelivery.MailStation.Criteria.MailReport;
using Fanex.MailDelivery.MailStation.Extensions;
using Fanex.MailDelivery.MailStation.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System.Web.Mvc;

namespace Fanex.MailDelivery.MailStation.Controllers
{
    [NapSuperAuth]
    public class MailReportController : BaseController
    {
        private readonly MailReportService mailReportService;

        public MailReportController()
        {
            mailReportService = new MailReportService();
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.ListStatus = mailReportService.TaskStatus();
            ViewBag.ListSite = mailReportService.Sites();
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            return View("~/Areas/MailStation/Report/MailReport/Index.cshtml");
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailReport(MailReportCriteria criteria)
        {
            return Json(mailReportService.MailReport(criteria));
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailDetail(int mailId)
        {
            var mailDetail = mailReportService.MailDetail(new MailDetailCriteria { MailId = mailId });
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            return View("~/Areas/MailStation/Report/MailDetail/Index.cshtml", mailDetail);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailLog(int mailId)
        {
            var mailLog = mailReportService.MailLog(new MailLogCriteria { MailId = mailId });
            ViewBag.TimeZoneString = TimeZoneExtension.GetTimeZoneString();

            return View("~/Areas/MailStation/Report/MailLog/Index.cshtml", mailLog);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Host)]
        public void AbortTask(int taskId)
        {
            mailReportService.AbortJob(taskId);
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Read)]
        public ActionResult RetrySendMail(RetrySendMailCriteria criteria)
        {
            mailReportService.RetrySendMail(criteria);

            return new EmptyResult();
        }

        [NapAuth(FunctionKey = MailDeliveryFunctionKeys.MailReport, Policy = Constant.Policy.Host)]
        public ActionResult RetryMissingTask(RetryMissingTaskCriteria criteria)
        {
            mailReportService.RetryMissingJob(criteria);

            return new EmptyResult();
        }
    }
}
