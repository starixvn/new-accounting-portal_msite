namespace Fanex.Maintenance.Service.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Model.LetstalkNotification;
    using ONEbook.NotificationService;

    public interface INotificationService
    {
        MessageModel Query(QueryModel model);
    }

    public class NotificationService : INotificationService
    {
        private readonly string _emailSuccess = "The email is sent successfully at " + DateTime.Now;
        private readonly string _emailUnSuccess = "The email is sent unsuccessfully at " + DateTime.Now;
        private readonly string _messageSuccess = "The message is sent successfully at " + DateTime.Now;
        private readonly string _messageUnSuccess = "The message is sent unsuccessfully at " + DateTime.Now;

        public MessageModel Query(QueryModel model)
        {
            var setting = ConfigurationManager.GetSection("LetstalkNotificationSetting") as Setting;
            var hasMailing = SendEmailByNotificationAPI(setting, model);
            var hasMessage = SendMessageByNotificationAPI(setting, model);

            return new MessageModel
            {
                Email = hasMailing ? _emailSuccess : _emailUnSuccess,
                Message = hasMessage ? _messageSuccess : _messageUnSuccess,
                EmailSuccess = hasMailing,
                MessageSuccess = hasMessage
            };
        }

        public bool SendEmailByNotificationAPI(Setting setting, QueryModel model)
        {
            var targetNameMaiing = model.IsPro ? setting.TargetNameMailing : setting.TargetNameMailing1;
            var targetName = targetNameMaiing + "?" + setting.ITALLMail;
            string[] serverUrls = { setting.NotificationService1, setting.NotificationService2 };

            var emailContent = model.Message + " at " + DateTime.Now;
            var contents = new Dictionary<string, string>
                {
                    { "Mail.Subject", "Testing" },
                    { "Email", emailContent }
                };

            Notification.Initialize(serverUrls, setting.AuthenticationKey);

            return Notification.Send(targetName, contents);
        }

        public bool SendMessageByNotificationAPI(Setting setting, QueryModel model)
        {
            var targetNameAlert = model.IsPro ? setting.TargetNameAlert : setting.TargetNameAlert1;
            var targetName = targetNameAlert + "?" + setting.ITALLAlert;
            string[] serverUrls = { setting.NotificationService1, setting.NotificationService2 };

            var contentMessage = model.Message + " at " + DateTime.Now;
            var contents = new Dictionary<string, string>
                {
                    { "Push.Subject", "Testing" },
                    { "Letstalk",  contentMessage }
                };

            Notification.Initialize(serverUrls, setting.AuthenticationKey);

            return Notification.Send(targetName, contents);
        }
    }
}
