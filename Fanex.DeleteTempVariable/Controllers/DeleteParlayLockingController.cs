using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.DeleteTempVariable.Model;
using Fanex.DeleteTempVariable.Requests;
using Fanex.DeleteTempVariable.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.DeleteTempVariable.Controllers
{
    [NapSuperAuth]
    public class DeleteParlayLockingController : BaseController
    {
        private readonly ParlayLockingService service;

        public DeleteParlayLockingController() : this(null)
        {
        }

        public DeleteParlayLockingController(ParlayLockingService service)
        {
            this.service = service ?? new ParlayLockingServiceImp();
        }

        [HttpGet, NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Views/TempVariable/ParlayLocking.cshtml", new ParlayLockingRequest());
        }

        [HttpPost, NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Read)]
        public ActionResult Index(ParlayLockingRequest request)
        {
            var matches =  service.GetMatches(request);
            ViewBag.Matches = matches;
            return View("~/Views/TempVariable/ParlayLocking.cshtml", request);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Host)]
        public ActionResult Delete(DeleteParlayLockingRequest request)
        {
            var result = service.DeleteMatch(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}