using System.Web.Mvc;
using Fanex.DeleteTempVariable.Requests;
using Fanex.DeleteTempVariable.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.DeleteTempVariable.Controllers
{
    [NapSuperAuth]
    public class DeleteTempVariableController : BaseController
    {
        private readonly MatchService service;

        public DeleteTempVariableController() : this(null)
        {
        }

        public DeleteTempVariableController(MatchService service)
        {
            this.service = service ?? new MatchServiceImp();
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Read)]
        public ActionResult Index(int? matchId)
        {
            var matches = service.GetMatches(matchId);
            ViewBag.Matches = matches;

            return View();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Read)]
        public ActionResult Detail(MatchTicketRequest request)
        {
            var ticketCount = service.TicketCount(request);
            var detail = service.GetMatchDetail(request.MatchId);
            detail.TicketCount = ticketCount;
            ViewBag.HasDelete = HasRight(SiteFunctionKeys.DeleteTempVariable, Constant.Policy.Host);
            return View(detail);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Host)]
        public ActionResult Delete(DeleteTempRequest request)
        {
            var result = service.DeleteMatch(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}