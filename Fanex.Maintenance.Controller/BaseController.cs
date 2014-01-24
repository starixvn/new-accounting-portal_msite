// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.1.4">Created</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using System.Configuration;
    using System;

    /// <summary>
    /// Base controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        protected readonly CurrentContext CurrentContext;

        protected readonly IServiceContext ServiceContext;

        protected readonly CurrentSession CurrentSession;

        private readonly IUserService _userService;

        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
            CurrentSession = new CurrentSession();

            ServiceContext = new ServiceContext
            {
                UserId = this.CurrentSession.UserId,
                Ip = this.CurrentContext.IP
            };

            ViewFlags = new ViewFlags
            {
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            _userService = new UserService { ServiceContext = ServiceContext };
        }

        /// <summary>
        /// Gets a value indicating whether the current site allows users login directly.
        /// </summary>
        /// <value>0 : users must login through NAP Hub.</value>
        public bool IsSiteLogin
        {
            get
            {
                return _issiteLogin;
            }
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = value;
            }
        }

        public PartialViewResult _Menu()
        {
            ViewBag.IsCSDUser = IsCSDUser(CurrentSession.UserId);
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }

        public bool HasRight(string functionKey, Constant.Policy policy)
        {
            int functionPolicy = _userService.GetUserSiteFunctionKeyPolicy(CurrentSession.UserId, this.SiteId, functionKey);

            return functionPolicy >= (int)policy;
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        protected bool IsCSDUser(int userId)
        {
            string strUserId = ConfigurationManager.AppSettings["CSDUserIds"];
            if (!string.IsNullOrWhiteSpace(strUserId))
            {
                string[] userIds = strUserId.Split(',');

                foreach (var item in userIds)
                {
                    if (item == userId.ToString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}