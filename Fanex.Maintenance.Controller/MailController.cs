namespace Fanex.Maintenance.Controller
{
    using Fanex.MailDeliver.Model;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Service.Criteria.MailService;
    using Service.Mail;

    [NapSuperAuth]
    public class MailController : BaseController
    {
        #region Member

        private readonly IMailService _mailServer;

        private readonly IUserService _userService;

        private readonly IMailRepository _mailRepository;

        #endregion Member

        #region Ctor

        public MailController()
            : this(null, null, null)
        {
        }

        public MailController(IMailService mailService, IUserService userService, IMailRepository mailRepository)
        {
            this._mailServer = mailService ?? MailService.Create();
            this._userService = userService ?? new UserService();
            this._mailRepository = mailRepository ?? new MailRepository();
        }

        #endregion Ctor

        #region Action

        #region MailList

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailList(int appId = 0)
        {
            InitPageSize();
            InitDataMailConfig(appId);
            InitMailStatus();

            var totalRows = 0;

            ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");

            var listMail = this._mailServer.WithApp(appId)
                .FromDate(DateTime.Now.ToShortDateString())
                .ToDate(DateTime.Now.ToShortDateString())
                .WithStatus(0)
                .WithPagination(1, 10)
                .GetListMailByDate(ref totalRows);

            return View(listMail);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailList(string from, string to, int applicationID = 0, int mailStatus = 0, int size = 10)
        {
            InitPageSize();

            InitDataMailConfig();

            InitMailStatus();

            var totalRows = 0;

            var dateFrom = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dateTo = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var listMail = this._mailServer.WithApp(applicationID)
                .FromDate(dateFrom.ToString())
                .ToDate(dateTo.ToString())
                .WithStatus(mailStatus)
                .WithPagination(1, size)
                .GetListMailByDate(ref totalRows);

            ViewBag.DateFrom = from;
            ViewBag.DateTo = to;

            return View(listMail);
        }

        #endregion MailList

        #region MailSetting

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Read)]
        public ActionResult MailSetting()
        {
            var listMailSetting = this._mailServer.GetListSetting();
            ViewBag.HasEdit = HasRight(SiteFunctionKeys.SmtpServers, Constant.Policy.Host);

            return View(listMailSetting);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult AddMailSetting()
        {
            return View(new MailSettingViewModel());
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult AddMailSetting(MailSettingViewModel model)
        {
            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                model.Password = model.Password == null ? string.Empty : model.Password;
                model.SettingName = model.Server + ":" + model.Port + " (" + model.Username + ")";
                var mailSetting = model.GetMailSetting();

                ViewBag.Success = this._mailServer.StoreSetting(mailSetting);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult AddMailSettingPopUp()
        {
            return View(new MailSetting());
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult EditMailSetting(int id)
        {
            var mailSetting = _mailServer.GetSetting(id);
            var model = new MailSettingViewModel(mailSetting);

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult EditMailSetting(MailSettingViewModel model)
        {
            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                model.Password = model.Password == null ? string.Empty : model.Password;
                model.SettingName = model.Server + ":" + model.Port + " (" + model.Username + ")";
                var mailSetting = model.GetMailSetting();
                ViewBag.Success = _mailServer.StoreSetting(mailSetting);
            }

            return View(model);
        }

        #endregion MailSetting

        #region MailConfig

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
        public ActionResult MailConfig(int id = 0)
        {
            var mailSetting = _mailServer.GetSetting(id);
            var serverIp = mailSetting != null ? mailSetting.Server : string.Empty;
            InitDataIpServer(serverIp);

            var listMailConfig = _mailServer.GetListConfig(serverIp);

            ViewBag.HasEdit = HasRight(SiteFunctionKeys.ApplicationManagement, Constant.Policy.Host);

            return View(listMailConfig);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
        public ActionResult MailConfig(string serverIp = "")
        {
            InitDataIpServer(serverIp);

            var listMailConfig = this._mailServer.GetListConfig(serverIp);

            ViewBag.HasEdit = HasRight(SiteFunctionKeys.ApplicationManagement, Constant.Policy.Host);

            return View(listMailConfig);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult AddMailConfig()
        {
            InitDataMailSetting();

            return View(new MailConfigViewModel());
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult AddMailConfig(MailConfigViewModel model)
        {
            InitDataMailSetting();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                var mailConfig = model.GetMailConfig();

                ViewBag.Success = this._mailServer.StoreConfig(mailConfig);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult AddMailConfigPopUp()
        {
            InitDataMailSetting();

            return View(new MailConfig());
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult EditMailConfig(int id)
        {
            InitDataMailSetting();

            ViewBag.Success = false;

            var mailConfig = _mailServer.GetConfig(id);
            var model = new MailConfigViewModel(mailConfig);

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult EditMailConfig(MailConfigViewModel model)
        {
            InitDataMailSetting();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                var mailConfig = model.GetMailConfig();

                ViewBag.Success = _mailServer.StoreConfig(mailConfig);
            }

            return View(model);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetMailConfigLogs(int appId)
        {
            var listMailConfig = _mailRepository.GetMailConfigLogs(appId);

            return View(listMailConfig);
        }

        #endregion MailConfig

        #region MailFunction

        [HttpGet]
        public ActionResult MailFunction()
        {
            var listMailFunction = _mailServer.GetListFunction();

            return View(listMailFunction);
        }

        [HttpGet]
        public ActionResult AddMailFunction()
        {
            InitDataMailSetting();
            InitDataMailConfig();

            return View(new MailFunctionViewModel());
        }

        [HttpPost]
        public ActionResult AddMailFunction(MailFunctionViewModel model)
        {
            InitDataMailSetting();
            InitDataMailConfig();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                var mailFunction = model.GetMailFunction();

                ViewBag.Success = _mailServer.StoreFunction(mailFunction);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditMailFunction(int id)
        {
            InitDataMailSetting();
            InitDataMailConfig();

            var mailFunction = _mailServer.GetFunction(id);

            var model = new MailFunctionViewModel(mailFunction);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditMailFunction(MailFunctionViewModel model)
        {
            InitDataMailSetting();
            InitDataMailConfig();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                var mailFunction = model.GetMailFunction();

                ViewBag.Success = _mailServer.StoreFunction(mailFunction);
            }

            return View(model);
        }

        #endregion MailFunction

        #region Compose Email

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ComposeEmail, Policy = Constant.Policy.Read)]
        public ActionResult ComposeEmail()
        {
            InitDataMailConfig();
            return View(new MailRecord());
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ComposeEmail, Policy = Constant.Policy.Read)]
        public ActionResult ComposeEmail(MailRecord mailRecord)
        {
            InitDataMailConfig();

            ConfigurationManager.AppSettings["app_mail_id"] = mailRecord.AppID.ToString();

            if (!string.IsNullOrWhiteSpace(mailRecord.FileAttachments))
            {
                var extension = Path.GetExtension(mailRecord.FileAttachments);
                FileExtentions fileExtension;

                switch (extension.ToLower())
                {
                    case ".xls":
                        fileExtension = FileExtentions.MS_EXCEL;
                        break;

                    case ".doc":
                        fileExtension = FileExtentions.MS_DOC;
                        break;

                    case ".jpg":
                        fileExtension = FileExtentions.IMG_JPG;
                        break;

                    case ".gif":
                        fileExtension = FileExtentions.IMG_GIF;
                        break;

                    case ".png":
                        fileExtension = FileExtentions.IMG_PNG;
                        break;

                    case ".pdf":
                        fileExtension = FileExtentions.PDF;
                        break;

                    default:
                        fileExtension = FileExtentions.TEXT;
                        break;
                }

                mailRecord.Attachments = new List<MailAttachment>();
                var attachment = new Fanex.MailDeliver.Model.MailAttachment(mailRecord.FileAttachments, fileExtension);
                mailRecord.Attachments.Add(attachment);
            }

            using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
            {
                client.SendMail(mailRecord);
            }

            ViewBag.Success = true;

            return View(mailRecord);
        }

        #endregion Compose Email

        #endregion Action

        #region Ajax

        [HttpPost]
        public ActionResult AddMailSettingPopUp(
            string server,
            int port,
            string username,
            string password,
            bool isAutho)
        {
            if (Request.IsAjaxRequest())
            {
                var mail = new MailSetting
                {
                    SettingName = server + ":" + port.ToString() + " (" + username + ") ",
                    Server = server,
                    Port = port,
                    Username = username,
                    Password = password,
                    IsAuth = isAutho
                };

                _mailServer.StoreSetting(mail);
                return Json(mail);
            }

            return Json(false);
        }

        [HttpPost]
        public ActionResult AddMailConfigPopUp(string appName, string description, int settingId)
        {
            var mailConfig = new MailConfig
            {
                AppName = appName,
                Description = description,
                Setting = new MailSetting
                {
                    SettingID = settingId
                }
            };

            InitDataMailSetting();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                ViewBag.Success = _mailServer.StoreConfig(mailConfig);
            }

            return Json(mailConfig);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ResendMail, Policy = Constant.Policy.Host)]
        public ActionResult ResendMail(int mailId)
        {
            if (Request.IsAjaxRequest())
            {
                var check = _mailServer.SendMail(mailId);
                return Json(check);
            }

            return Json(false);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ResendMail, Policy = Constant.Policy.Host)]
        public ActionResult ResendMultipleMails(int[] mailIds)
        {
            if (Request.IsAjaxRequest())
            {
                foreach (var mailId in mailIds)
                {
                    if (mailId > 0)
                    {
                        _mailServer.SendMail(mailId);
                    }
                }
            }

            return new EmptyResult();
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailDetails, Policy = Constant.Policy.Read)]
        public ActionResult GetMessage(int mailId)
        {
            if (Request.IsAjaxRequest())
            {
                var mail = _mailServer.GetMail(mailId);
                return View(mail);
            }

            return View(new MailRecord());
        }

        [HttpPost]
        public ActionResult GetError(int mailId)
        {
            if (Request.IsAjaxRequest())
            {
                var mail = _mailServer.GetError(mailId);
                return View(mail);
            }

            return View(new List<MailError>());
        }

        [HttpPost]
        public ActionResult MailListPagi(
            int status,
            string from,
            string to,
            int page = 0,
            int size = 50,
            int applicationID = 0)
        {
            InitDataMailConfig();

            var totalRows = 0;

            var dateFrom = !string.IsNullOrWhiteSpace(from) ? DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
            var dateTo = !string.IsNullOrWhiteSpace(from) ? DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;

            var listMail = _mailServer.WithApp(applicationID)
                .FromDate(dateFrom.ToString())
                .ToDate(dateTo.ToString())
                .WithPagination(page, size)
                .WithStatus(status)
                .GetListMailByDate(ref totalRows);

            var policyViewDetail = _userService.GetPermission(CurrentSession.UserId, this.SiteId, SiteFunctionKeys.ViewMailDetails);
            var policyResend = _userService.GetPermission(CurrentSession.UserId, this.SiteId, SiteFunctionKeys.ResendMail);

            return Json(new
            {
                TotalRows = totalRows,
                HasViewDetail = policyViewDetail,
                HasResend = policyResend,
                ListMail = listMail
            });
        }

        [HttpPost]
        public ActionResult GetMailSetting()
        {
            if (Request.IsAjaxRequest())
            {
                var listMailSetting = _mailServer.GetListSetting();

                return Json(listMailSetting);
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult GetMailConfig()
        {
            if (Request.IsAjaxRequest())
            {
                var listMailConfig = _mailServer.GetListConfig();

                return Json(listMailConfig);
            }

            return Json(null);
        }

        #endregion Ajax

        #region InitData

        private void InitPageSize()
        {
            var listPageSize = new List<SelectListItem>();
            listPageSize.Add(new SelectListItem
            {
                Text = "10",
                Value = "10"
            });

            listPageSize.Add(new SelectListItem
            {
                Text = "30",
                Value = "30"
            });

            listPageSize.Add(new SelectListItem
            {
                Text = "50",
                Value = "50"
            });

            listPageSize.Add(new SelectListItem
            {
                Text = "100",
                Value = "100"
            });

            ViewBag.PageSize = listPageSize;
        }

        private void InitMailStatus()
        {
            var listMailStatus = new List<SelectListItem>();
            listMailStatus.Add(new SelectListItem
            {
                Text = MailStatus.All.ToString(),
                Value = ((int)MailStatus.All).ToString()
            });

            listMailStatus.Add(new SelectListItem
            {
                Text = MailStatus.Fail.ToString(),
                Value = ((int)MailStatus.Fail).ToString()
            });

            listMailStatus.Add(new SelectListItem
            {
                Text = MailStatus.Success.ToString(),
                Value = ((int)MailStatus.Success).ToString()
            });

            ViewBag.MailStatus = listMailStatus;
        }

        private void InitDataMailConfig(int appId = 0)
        {
            var listConfig = _mailServer.GetListConfig(string.Empty);
            var listConfigPermission = new List<MailConfig>();

            var configPermission = ConfigurationManager.AppSettings["MailPermission:Application"];
            var applicationPermissions = configPermission.Split(';');

            foreach (var itemApplicationPermission in applicationPermissions)
            {
                var permission = itemApplicationPermission.Split(':');
                var functionKey = permission[0];
                var appliationId = int.Parse(permission[1]);

                var hasPermission = HasRight(functionKey, Constant.Policy.Read)
                    && CurrentSession.Level == (int)Constant.Level.User;

                if (hasPermission)
                {
                    listConfigPermission.AddRange(listConfig.Where(x => x.AppID == appliationId));
                }
            }

            if (listConfigPermission.Any())
            {
                listConfig = listConfigPermission;
            }

            ViewBag.HasLimitedConfigPermission = listConfigPermission.Any();

            ViewBag.ListMailConfig = listConfig.Select(c => new SelectListItem
            {
                Text = c.AppName,
                Value = c.AppID.ToString(),
                Selected = c.AppID == appId
            }).ToList();
        }

        private void InitDataMailSetting()
        {
            var listMailSetting = _mailServer.GetListSetting();

            ViewBag.ListMailSetting = listMailSetting.Select(s => new SelectListItem
            {
                Text = s.SettingName,
                Value = s.SettingID.ToString(),
            }).ToList();
        }

        private void InitDataIpServer(string serverIp = "")
        {
            var listMailSetting = _mailServer.GetListSetting();
            listMailSetting = listMailSetting.GroupBy(s => s.Server).Select(s => s.First()).ToList();

            ViewBag.ListServerIp = listMailSetting.Select(s => new SelectListItem
            {
                Text = s.Server,
                Value = s.Server,
                Selected = s.Server == serverIp
            }).ToList();
        }

        #endregion InitData
    }
}
