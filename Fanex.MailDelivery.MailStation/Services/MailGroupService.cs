namespace Fanex.MailDelivery.MailStation.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Core.Command;

    /// <summary>
    /// Mail Report Service.
    /// </summary>
    public class MailGroupService : MailStationBaseService
    {
        public MailGroupService()
        {
        }

        public List<MailGroupModel> GetMailGroup(GetMailGroupCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailGroupModel>().ToList();
            }
        }

        public CommandResult CreateMailGroup(CreateMailGroupCriteria criteria)
        {
            return this.ExecuteNonQuery(criteria);
        }

        public CommandResult UpdateMailGroup(UpdateMailGroupCriteria criteria)
        {
            return this.ExecuteNonQuery(criteria);
        }

        public CommandResult DeleteMailGroup(DeleteMailGroupCriteria criteria)
        {
            return this.ExecuteNonQuery(criteria);
        }

        public MailGroupModel GetMailGroupByID(GetMailGroupByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailGroupModel>().FirstOrDefault();
            }
        }
    }
}
