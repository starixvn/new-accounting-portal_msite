namespace Fanex.Tool.TraceSP.Controllers
{
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Fanex.Maintenance.Controller;
    using Models;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services;

    public class TracingSpController : BaseController
    {
        private readonly IDbServerService _dbServerService;
        private readonly ITracingSpService _tracingSpService;
        private readonly ISiteService _siteService;
        private readonly string _dbServerXmlPath;
        private readonly string _siteXmlPath;

        public TracingSpController()
        {
            _dbServerService = new DbServerService();
            _dbServerXmlPath = HostingEnvironment.MapPath(AppSettingService.DbListXMLPath);
            _siteXmlPath = HostingEnvironment.MapPath(AppSettingService.SiteXMLPath);
            _tracingSpService = new TracingSpService();
            _siteService = new SiteService();
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.TracingSP, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var servers = _dbServerService.GetDbServerList(_dbServerXmlPath);
            var sites = _siteService.GetSites(_siteXmlPath);
            var model = new TracingSp {
                Servers = servers.Select(x=> new Server { Ip = x.Ip , Name = x.Name + " (" + x.Ip +")" }).ToList(),
                Sites = sites
            };

            return View("~/Areas/TracingSp/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.TracingSP, Policy = Constant.Policy.Read)]
        public ActionResult GetTracingSpData(TracingSp criteria)
        {
            var model = new TracingSp
            {
                Mode = false,
                TraceStartEvents = criteria.TraceStartEvents,
                Server = criteria.Server,
                Site = criteria.Site,
                TextDataLike = criteria.TextDataLike,
                TextDataLike2 = criteria.TextDataLike2,
                TextDataLike3 = criteria.TextDataLike3,
                TraceTimer = criteria.TraceTimer              
            };

            var tracingSpData = _tracingSpService.GetTracingSpData(model);

            return Json(tracingSpData, JsonRequestBehavior.AllowGet);
        }
    }
}
