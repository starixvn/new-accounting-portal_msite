namespace Fanex.Tool.TraceSP.Controllers
{
    using Maintenance.Controller;
    using Models;
    using Services;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;

    public class TracingSpController : BaseController
    {
        private readonly IDbServerService _dbServerService = new DbServerService();
        private readonly ITracingSpService _tracingSpService = new TracingSpService();
        private readonly ISiteService _siteService = new SiteService();
        private readonly string _dbServerXmlPath = HostingEnvironment.MapPath(AppSettingService.DbListXMLPath);
        private readonly string _siteXmlPath = HostingEnvironment.MapPath(AppSettingService.SiteXMLPath);

        //[NapAuth(FunctionKey = SiteFunctionKeys.TracingSP, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var servers = _dbServerService.GetDbServerList(_dbServerXmlPath);
            var sites = _siteService.GetSites(_siteXmlPath);
            var model = new TracingSp
            {
                Servers = servers.Select(x => new Server { Ip = x.Ip, Name = x.Name + " (" + x.Ip + ")" }).ToList(),
                Sites = sites
            };

            return View("~/Areas/TracingSp/Index.cshtml", model);
        }

        [HttpPost]
        //[NapAuth(FunctionKey = SiteFunctionKeys.TracingSP, Policy = Constant.Policy.Read)]
        public ActionResult GetTracingSpData(TracingSp criteria)
        {
            var model = new TracingSp
            {
                Mode = false,
                TraceStartEvents = criteria.TraceStartEvents,
                Server = criteria.Server,
                Site = criteria.Site,
                TextDataLike = AddPercentSign(criteria.TextDataLike),
                TextDataLike2 = AddPercentSign(criteria.TextDataLike2),
                TextDataLike3 = AddPercentSign(criteria.TextDataLike3),
                TraceTimer = criteria.TraceTimer,
                ErrorMsg = string.Empty,
                HasError = false
            };

            var tracingSpData = _tracingSpService.GetTracingSpData(model);

            return Json(new
            {
                Data = tracingSpData,
                model.HasError,
                model.ErrorMsg
            }, JsonRequestBehavior.AllowGet);
        }

        private static string AddPercentSign(string textLike)
        => !string.IsNullOrWhiteSpace(textLike) && !textLike.Contains("%") ? $"%{textLike}%" : textLike;
    }
}
