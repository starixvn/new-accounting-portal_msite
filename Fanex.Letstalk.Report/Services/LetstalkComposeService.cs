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
    public interface LetstalkComposeService
    {
        CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files);

        IEnumerable<SelectListItem> GetGroups();
    }

    public class LetstalkComposeServiceImp : LetstalkComposeService
    {
        public CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<HttpPostedFileBase> files)
        {
            Hashtable hash = criteria.GetSpParams();
            int notificationID = 0;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                notificationID = Converter.ToInt(db.GetParameterValue("NotificationID"));
            }

            foreach (var file in files)
            {
                InsertAttachementFileData(notificationID, file);
            }

            return new CommandResult
            {
                Code = 0,
                Message = string.Empty
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
                        Selected = true,
                        Value = recipient.RecipientID.ToString(),
                        Text = recipient.ConversationID + " - " + recipient.RecipientName
                    });
                }

            }
            return result;
        }
    }
}
