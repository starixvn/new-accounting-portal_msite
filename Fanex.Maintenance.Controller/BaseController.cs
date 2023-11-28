namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using NAP.Core.Business;
    using NAP.Core.Configuration;
    using NAP.Core.Context;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using NAP.Core.Web;
    using NAP.Core.Web.Session;

    public class BaseController : Controller
    {
        protected readonly string Environment = NapSettings.Settings.Environment;
        protected readonly int SiteId = NapSettings.Settings.SiteId;
        protected readonly CurrentContext CurrentContext;
        protected readonly CurrentSession CurrentSession;
        private readonly IUserService _userService;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
            CurrentSession = new CurrentSession();

            ViewFlags = new ViewFlags
            {
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            _userService = new UserService();
        }

        protected ViewFlags ViewFlags
        {
            get => ViewBag.ViewFlags;

            set => ViewBag.ViewFlags = value;
        }

        public PartialViewResult _Menu()
        {
            ViewBag.Menu = CurrentSession.MenuItemList;
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }

        public bool HasRight(string functionKey, Constant.Policy policy)
        {
            var functionPolicy = _userService.GetPermission(CurrentSession.UserId, SiteId, functionKey);

            return functionPolicy >= (int)policy;
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}
