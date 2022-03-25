using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Common;
using Fanex.Data;
using Fanex.Letstalk.Notification.Criteria;
using Fanex.Letstalk.Notification.Model;
using Fanex.Maintenance.Repository.Command;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Service.Interface;

namespace Fanex.Letstalk.Notification.Services
{
    public interface ManageLetstalkMessageService
    {
        CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files);

        IEnumerable<SelectListItem> GetGroups();

        CommandResult CancelNotificationId(int notificationId);

        CommandResult UpdateNotificationStatus(UpdateNotificationStatusCriteria criteria);

        IEnumerable<SelectListItem> GetStatusList();

        IEnumerable<NotificationModel> GetNotificationList(GetNotificationListCriteria criteria);

        IEnumerable<NotificationModel> GetNotificationById(GetNotificationListCriteria criteria);

        CommandResult EditNotification(EditNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files);

        CommandResult DeleteNotification(DeleteNotificationCriteria criteria);
    }

    public class LetstalkComposeServiceImp : ManageLetstalkMessageService
    {

        private readonly IUserService userService;

        public LetstalkComposeServiceImp()
        {
            userService = new UserService();
        }

        public CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files)
        {
            Hashtable hash = criteria.GetSpParams();
            //int notificationID = 0;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                //notificationID = Converter.ToInt(db.GetParameterValue("NotificationID"));
            }

            //foreach (var file in files)
            //{
            //    InsertAttachementFileData(notificationID, file);
            //}

            return new CommandResult
            {
                Code = 0,
                Message = "Message has been created successfully!"
            };
        }

        private void InsertAttachementFileData(int notificationId, HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] bytes = target.ToArray();
            var criteria = new InsertAttachmentFileCriteria
            {
                ContentType = file.ContentType,
                NotificationID = notificationId,
                FileName = file.FileName,
                Content = bytes
            };

            Hashtable hash = criteria.GetSpParams();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }
        }

        public IEnumerable<SelectListItem> GetGroups()
        {
            GetGroupCriteria criteria = new GetGroupCriteria();
            List<RecipientModel> recipientList;
            List<SelectListItem> result = new List<SelectListItem>();
            var environment = ConfigurationManager.AppSettings["Environment"];
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                recipientList = db.Query<RecipientModel>().ToList();
            }
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

        public CommandResult CancelNotificationId(int notificationId)
        {
            var criteria = new CancelNotificationCriteria { LstNotificationID = notificationId.ToString() };
            Hashtable hash = criteria.GetSpParams();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
            };
        }

        public CommandResult UpdateNotificationStatus(UpdateNotificationStatusCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
            };
        }

        public IEnumerable<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Waiting" , Value = "0" },
                new SelectListItem { Text = "Sending" , Value = "1" },
                new SelectListItem { Text = "Successful" , Value = "2" },
                new SelectListItem { Text = "Failed" , Value = "3" }
            };
        }

        public IEnumerable<NotificationModel> GetNotificationList(GetNotificationListCriteria criteria)
        {
            List<NotificationModel> notificationList;
            List<NotificationRecipientModel> notificationRecipientList;
            var param = criteria.GetSpParams();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var multi = db.QueryMultiple(param);
                notificationList = multi.Read<NotificationModel>().ToList();
                notificationRecipientList = multi.Read<NotificationRecipientModel>().ToList();
            }
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
                foreach(var notificationRecipient in notificationRecipientList)
                {
                    if (notificationRecipient.NotificationID == item.NotificationID)
                    {
                        var temp = item;
                        temp.RecipientID = notificationRecipient.RecipientID;
                        temp.RecipientName = notificationRecipient.RecipientName;
                        temp.ConversationID = notificationRecipient.ConversationID;
                        temp.Status = notificationRecipient.Status;
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
            List<NotificationModel> result;
            var param = criteria.GetSpParams();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<NotificationModel>(param).ToList();
            }
            return result;
        }

        public CommandResult EditNotification(EditNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);
                }

                return new CommandResult
                {
                    Code = 0,
                    Message = "Message has been edited successfully!"
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
                Hashtable hash = criteria.GetSpParams();
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);
                }

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
    }
}
