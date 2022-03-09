using Fanex.Data;
using Fanex.Letstalk.Notification.Criteria;
using Fanex.Letstalk.Notification.Model;
using Fanex.NAP.Core.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Letstalk.Notification.Services
{
    public interface LetstalkComposeService
    {
        CommandResult InsertNotification(InsertNotificationCriteria criteria);

        IEnumerable<SelectListItem> GetGroups();
    }

    public class LetstalkComposeServiceImp : LetstalkComposeService
    {
        public CommandResult InsertNotification(InsertNotificationCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                //Converter.ToInt(db.GetParameterValue("NotificationID"));

                return GetCommandResult(hash);
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
            foreach(var recipient in recipientList)
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

        public CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];
            return new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }
    }
}
