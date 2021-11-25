using System.Web.Mvc;
using Fanex.DeleteTempVariable.Requests;
using Fanex.DeleteTempVariable.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.DeleteTempVariable.Controllers
{
    [NapSuperAuth]
    public class DeleteSettlementLockController : BaseController
    {
        private readonly SettlementLockService service;

        public DeleteSettlementLockController() : this(null)
        {
        }

        public DeleteSettlementLockController(SettlementLockService service)
        {
            this.service = service ?? new SettlementLockServiceImp();
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Read)]
        public ActionResult Index(DeleteTempRequest request)
        {
            var matches = service.GetMatches(request);
            ViewBag.Matches = matches;
            return View("~/Views/TempVariable/SettlementLock.cshtml", new DeleteTempRequest());
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Host)]
        public ActionResult Delete(DeleteSettlementProcessingRequest request)
        {
            var result =service.Delete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdateSettlementLogRequest request)
        {
            var result = service.UpdateSettlementLogs(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
