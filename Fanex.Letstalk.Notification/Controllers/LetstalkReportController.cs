using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Letstalk.Notification.Params;
using Fanex.Letstalk.Notification.Services;

namespace Fanex.Letstalk.Notification.Controllers
{
    public class LetstalkReportController : Controller
    {
        private readonly ILetstalkReportRestClientService service;

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
        public ActionResult GetTargets(HttpGetLogParams httpParams)
        {
            var queryParams = new GetLogParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate,
                Id = 0
            };

            var result = service.GetTargets(queryParams);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetGroupsByTarget(HttpGetLogParams httpParams)
        {
            var queryParams = new GetLogParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate,
                TargetName = httpParams.TargetName,
                Id = 0
            };

            var result = service.GetGroupsByTarget(queryParams);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReport(HttpGetLogParams httpParams)
        {
            var queryParams = new GetLogParams
            {
                FromDate = httpParams.FromDate,
                ToDate = httpParams.ToDate.AddDays(1),
                TargetName = httpParams.TargetName,
                Group = httpParams.Group,
                Id = 0,
                Status = httpParams.Status
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

        [HttpPost]
        public ActionResult ResendMultiple(HttpResendMultipleParams httpParams)
        {
	        var result = new List<bool>();
	        foreach (var item in httpParams.Data)
            {
	            var resendParams = new ResendParams
	            {
		            FromDate = httpParams.FromDate,
		            ToDate = httpParams.ToDate,
		            Id = item.Id
	            };
				var temp = item.IsMessage ? service.ReSendMessage(resendParams) : service.ReSendMail(resendParams);
				if (!temp)
				{
                    result.Add(false);
				}
			}

	        return Json(result, JsonRequestBehavior.AllowGet);
        }

	}
}
