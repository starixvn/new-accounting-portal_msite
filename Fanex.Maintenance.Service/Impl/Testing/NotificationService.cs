namespace Fanex.Maintenance.Service.Impl.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Core.Model.LetstalkNotification;
    using Interface.Testing;
    using ONEbook.NotificationService;

    public class NotificationService: INotificationService
    {
        private string emailSuccess = "The email is sent successfully at " + DateTime.Now.ToString();
        private string emailUnSuccess = "The email is sent unsuccessfully at " + DateTime.Now.ToString();
        private string messageSuccess = "The message is sent successfully at " + DateTime.Now.ToString();
        private string messageUnSuccess = "The message is sent unsuccessfully at " + DateTime.Now.ToString();

        public MessageModel Query(QueryModel model)
        {
            var setting = InitSetting();
            var hasMailing = SendEmailByNotificationAPI(setting, model);
            var hasMessage = SendMessageByNotificationAPI(setting, model);

            return new MessageModel
            {
                Email = hasMailing == true ? emailSuccess : emailUnSuccess,
                Message = hasMessage == true ? messageSuccess : messageUnSuccess,
                EmailSuccess = hasMailing,
                MessageSuccess = hasMessage
            };
        }

        public Setting InitSetting()
        {
            return ConfigurationManager.GetSection("LetstalkNotificationSetting") as Setting;
        }

        public bool SendEmailByNotificationAPI(Setting setting, QueryModel model)
        {
            bool result = false;
            var targetNameMaiing = model.IsPro == true ? setting.TargetNameMailing : setting.TargetNameMailing1;

            try
            {
                string targetName = targetNameMaiing + "?" + setting.ITALLMail;
                string[] serverUrls = new string[] { setting.NotificationService1, setting.NotificationService2 };
                Notification.Initialize(serverUrls, setting.AuthenticationKey);

                var emailContent = model.Message + " at " + DateTime.Now.ToString();
                var contents = new Dictionary<string, string>()
                {
                    { "Mail.Subject", "Testing" },
                    { "Email", emailContent }
                };

                result = Notification.Send(targetName, contents, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool SendMessageByNotificationAPI(Setting setting, QueryModel model)
        {
            bool result = false;
            var targetNameAlert = model.IsPro == true ? setting.TargetNameAlert : setting.TargetNameAlert1;

            try
            {
                string targetName = targetNameAlert + "?" + setting.ITALLAlert;
                string[] serverUrls = new string[] { setting.NotificationService1, setting.NotificationService2 };
                Notification.Initialize(serverUrls, setting.AuthenticationKey);

                var contentMessage = model.Message + " at " + DateTime.Now.ToString();
                var contents = new Dictionary<string, string>()
                     {
                         { "Push.Subject",  "Testing" },
                         { "Letstalk",  contentMessage }
                     };

                result = Notification.Send(targetName, contents, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}