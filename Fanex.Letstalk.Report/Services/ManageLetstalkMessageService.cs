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
    }

    public class LetstalkComposeServiceImp : ManageLetstalkMessageService
    {
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
                new SelectListItem { Text = "All" , Value = "" },
                new SelectListItem { Text = "Waiting" , Value = "0" },
                new SelectListItem { Text = "Sending" , Value = "1" },
                new SelectListItem { Text = "Successful" , Value = "2" },
                new SelectListItem { Text = "Failed" , Value = "3" }
            };
        }

        public IEnumerable<NotificationModel> GetNotificationList(GetNotificationListCriteria criteria)
        {
            List<NotificationModel> result;
            var param = criteria.GetSpParams();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<NotificationModel>(param).ToList();
            }
            return result;
        }
    }
}
