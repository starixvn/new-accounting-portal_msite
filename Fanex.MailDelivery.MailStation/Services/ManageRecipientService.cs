using Fanex.Data;
using Fanex.MailDelivery.MailStation.Criteria;
using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.NAP.Core.Command;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Service.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.MailDelivery.MailStation.Services.ManageRecipient
{
    public class ManageRecipientService : MailStationBaseService
    {
        private readonly IUserService userService;

        public ManageRecipientService()
        {
            userService = new UserService();
        }
      
        public List<RecipientModel> RecipientList(RecipientListCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().ToList();
            }
        }
      
        public RecipientModel RecipientById(RecipientByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().FirstOrDefault();
            }
        }

        public CommandResult UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public CommandResult CreateRecipient(CreateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public CommandResult DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        public User CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            return userService.Get(criteria.Username);
        }
    }
}