using System.Web.Mvc;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.DeleteTempVariable.Criteria;
using Fanex.DeleteTempVariable.Model;
using Fanex.DeleteTempVariable.Services;
using Fanex.Maintenance.Controller;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.DeleteTempVariable.Controllers
{
    [NapSuperAuth]
    public class UpdateSystemSettingsController : BaseController
    {
        private readonly IDynamicRepository repository;

        public UpdateSystemSettingsController() : this(null)
        {
        }

        public UpdateSystemSettingsController(MatchService service)
        {
            this.repository = new DynamicRepository();
        }

        [HttpGet, NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var setting = repository.Get<SystemSettings>(new GetSystemSettingCriteria());
            ViewBag.SystemSetting = setting;
            return View("~/Views/TempVariable/SystemSettings.cshtml");
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DeleteTempVariable, Policy = Constant.Policy.Host)]
        public ActionResult Index(int? id)
        {
            var criteria = new UpdateSystemSettingCriteria();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery();
            }

            return this.Index();
        }
    }
}