using Fanex.Letstalk.Notification.Criteria;
using Fanex.Letstalk.Notification.Model;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Service.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Letstalk.Notification.Services
{
    using Command;
    using Data.Repository;

    public interface ManageLetstalkMessageService
    {
        CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files);

        IEnumerable<SelectListItem> GetGroups();

        IEnumerable<SelectListItem> GetStatusList();

        IEnumerable<NotificationModel> GetNotificationList(GetNotificationListCriteria criteria);

        IEnumerable<NotificationModel> GetNotificationById(GetNotificationListCriteria criteria);

        CommandResult EditNotification(EditNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files);

        CommandResult DeleteNotification(DeleteNotificationCriteria criteria);

        CommandResult ResendNotification(ResendNotificationCriteria criteria);
    }

    public class ManageLetstalkMessageServiceImp : ManageLetstalkMessageService
    {

        private readonly IUserService userService = new UserService();

        private readonly string environment = ConfigurationManager.AppSettings["Environment"];

        private readonly string uatEnvironment = "UAT";

        private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

        public CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files)
        {
            if (environment == uatEnvironment)
            {
                criteria.IsTest = true;
            }

            dynamicRepository.Execute(criteria);

            return new CommandResult
            {
                Code = 0,
                Message = ""
            };
        }

        public IEnumerable<SelectListItem> GetGroups()
        {
            var criteria = new GetGroupCriteria();
            var result = new List<SelectListItem>();
            var recipientList = dynamicRepository.Fetch<RecipientModel>(criteria).ToList();

            foreach (var recipient in recipientList)
            {
                if (recipient.Environment.Equals(environment, StringComparison.CurrentCultureIgnoreCase))
                {
                    result.Add(new SelectListItem
                    {
                        Selected = false,
                        Value = recipient.RecipientID.ToString(),
                        Text = recipient.ConversationID + " - " + recipient.RecipientName
                    });
                }

            }
            return result;
        }

        public IEnumerable<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Waiting" , Value = "0" },
                new SelectListItem { Text = "Sending" , Value = "1" },
                new SelectListItem { Text = "Sent" , Value = "2" },
                new SelectListItem { Text = "Failed" , Value = "3" }
            };
        }

        public IEnumerable<NotificationModel> GetNotificationList(GetNotificationListCriteria criteria)
        {
            if (environment == uatEnvironment)
            {
                criteria.IsTest = true;
            }

            var (notificationList, notificationRecipientList) =dynamicRepository.FetchMultiple<NotificationModel, NotificationRecipientModel>(criteria);

            var result = UpdateModelView(notificationList, notificationRecipientList);
            return result;
        }

        private IEnumerable<NotificationModel> UpdateModelView(IEnumerable<NotificationModel> notificationList, IEnumerable<NotificationRecipientModel> notificationRecipientList)
        {
            var result = new List<NotificationModel>();
            foreach (var item in notificationList)
            {
                item.Creator = GetUserName(item.CreatedBy);
                item.LatestEditor = GetUserName(item.LastModifiedBy);
                foreach (var notificationRecipient in notificationRecipientList)
                {
                    if (notificationRecipient.NotificationID == item.NotificationID)
                    {
                        var temp = item;
                        temp.RecipientID = notificationRecipient.RecipientID;
                        temp.RecipientName = notificationRecipient.RecipientName;
                        temp.ConversationID = notificationRecipient.ConversationID;
                        temp.Status = notificationRecipient.Status;
                        temp.LastSentTime = notificationRecipient.LastSentTime;
                        result.Add(temp);
                    }
                }
            }
            return result;
        }

        private string GetUserName(int userId)
        {
            try
            {
                return userService.Get(userId)?.Username;
            }
            catch
            {
                return "";
            }
        }

        public IEnumerable<NotificationModel> GetNotificationById(GetNotificationListCriteria criteria)
        {
            if (environment == uatEnvironment)
            {
                criteria.IsTest = true;
            }

            return dynamicRepository.Fetch<NotificationModel>(criteria);
        }

        public CommandResult EditNotification(EditNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                dynamicRepository.Execute(criteria);

                return new CommandResult
                {
                    Code = 0,
                    Message = ""
                };
            }
            catch
            {
                return new CommandResult
                {
                    Code = 1,
                    Message = "You are not allowed to update in-progress messages. All changes will be discarded."
                };
            }
        }

        public CommandResult DeleteNotification(DeleteNotificationCriteria criteria)
        {
            try
            {
                dynamicRepository.Execute(criteria);

                return new CommandResult
                {
                    Code = 0,
                    Message = "Message has been deleted successfully!"
                };
            }
            catch
            {
                return new CommandResult
                {
                    Code = 1,
                    Message = "You are not allowed to delete in-progress messages. All changes will be discarded."
                };
            }
        }

        public CommandResult ResendNotification(ResendNotificationCriteria criteria)
        {
            try
            {
                dynamicRepository.Execute(criteria);

                return new CommandResult
                {
                    Code = 0,
                    Message = "Message has been resent successfully!"
                };
            }
            catch
            {
                return new CommandResult
                {
                    Code = 1,
                    Message = "You are not allowed to resend in-progress messages. All changes will be discarded."
                };
            }
        }
    }
}
