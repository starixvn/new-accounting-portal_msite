using System.Web.Mvc;
using Fanex.Letstalk.Notification.Params;
using Fanex.Letstalk.Notification.Services;
using Fanex.Maintenance.Controller;

namespace Fanex.Letstalk.Notification.Controllers
{
    public class LetstalkReportController : BaseController
    {
        private ILetstalkReportRestClientService service;

        public LetstalkReportController() : this(null)
        {
        }

        public LetstalkReportController(ILetstalkReportRestClientService service)
        {
            this.service = service ?? new LetstalkReportRestClientService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/LetstalkReport/Index.cshtml");
        }

        [HttpPost]
        public ActionResult GetReport(HttpGetLogParams httpParams)
        {
            var queryParams = new GetLogParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate.AddDays(1),
                Id = 0
            };

            var result = service.GetLogs(queryParams);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReSendMessage(HttpResendParams httpParams)
        {
            var resendParams = new ResendParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate,
                Id = httpParams.Id
            };

            var result = service.ReSendMessage(resendParams);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReSendMail(HttpResendParams httpParams)
        {
            var resendParams = new ResendParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate,
                Id = httpParams.Id
            };

            var result = service.ReSendMail(resendParams);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}