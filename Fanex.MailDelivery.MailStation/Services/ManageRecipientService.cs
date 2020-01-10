using Fanex.Data;
using Fanex.MailDelivery.MailStation.Criteria;
using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
using Fanex.MailDelivery.MailStation.Models;
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

        /// <summary>
        /// Edits the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Editing result.</returns>
        public CommandResult EditRecipient(UpdateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Inserts the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Inserting result.</returns>
        public CommandResult InsertRecipient(CreateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Deletes the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result.</returns>
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

        /// <summary>
        /// Checks the recipient information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient information.</returns>
        public User CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            return userService.Get(criteria.Username);
        }
    }
}