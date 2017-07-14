namespace Fanex.Tool.TraceSP.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Data.Configuration;
    using Fanex.Maintenance.Controller;
    using Models;
    using Services;

    public class TracingSpController : BaseController
    {
        private readonly IDbServerService _dbServerService;
        private readonly ITracingSpService _tracingSpService;
        private readonly string _dbServerXmlPath;

        public TracingSpController()
        {
            _dbServerService = new DbServerService();
            _dbServerXmlPath = HostingEnvironment.MapPath(AppSettingService.DbListXMLPath);
            _tracingSpService = new TracingSpService();
        }

        public JsonResult GetDbServerList()
        {
            var dbServers = new List<Server>();
            if (Request.IsAjaxRequest())
            {
                dbServers = _dbServerService.GetDbServerList(_dbServerXmlPath);
            }

            return Json(dbServers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View("~/Areas/TracingSp/Index.cshtml");
        }

        public ActionResult GetTracingSpData(TracingSp model)
        {

            var tracingSpData = _tracingSpService.GetTracingSpData(model);

            return Json(tracingSpData, JsonRequestBehavior.AllowGet);
        }
    }
}
