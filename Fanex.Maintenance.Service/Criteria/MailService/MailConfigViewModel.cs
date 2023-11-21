namespace Fanex.Maintenance.Service.Criteria.MailService
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Maintenance.Core;
    using MailDeliver.Model;
    using NAP.Core.Web.Session;

    /// <summary>
    /// The mail config view model class.
    /// </summary>
    public class MailConfigViewModel
    {
        public MailConfigViewModel()
        {
        }

        public MailConfigViewModel(MailConfig mailSetting)
        {
            this.AppName = mailSetting.AppName;
            this.Description = mailSetting.Description;
            this.SettingId = mailSetting.Setting.SettingID;
            this.AppId = mailSetting.AppID;
            this.MailFrom = string.IsNullOrEmpty(mailSetting.MailFrom) ? mailSetting.Setting.Username : mailSetting.MailFrom;
            this.IsEncrypt = mailSetting.IsEncrypt;
        }

        public int AppId { get; set; }

        [Required]
        [Display(Name = "Setting", ResourceType = typeof(ViewResource))]
        public int SettingId { get; set; }

        [Required]
        [Display(Name = "Application_Name", ResourceType = typeof(ViewResource))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        public string AppName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewResource))]
        public string Description { get; set; }

        [Display(Name = "MailFrom", ResourceType = typeof(ViewResource))]
        public string MailFrom { get; set; }

        [Display(Name = "IsEncrypt", ResourceType = typeof(ViewResource))]
        public bool IsEncrypt { get; set; }

        /// <summary>
        /// Get mail config.
        /// </summary>
        /// <returns>Mail config.</returns>
        public MailConfig GetMailConfig()
        {
            var mailConfig = new MailConfig
            {
                AppID = this.AppId,
                AppName = this.AppName,
                Description = this.Description,
                MailFrom = this.MailFrom,
                IsEncrypt = this.IsEncrypt,
                Setting = new MailSetting
                {
                    SettingID = this.SettingId
                },
                UserId = new CurrentSession().UserId
            };

            return mailConfig;
        }
    }
}