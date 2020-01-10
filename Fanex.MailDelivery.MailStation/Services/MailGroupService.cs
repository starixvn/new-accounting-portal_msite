using Fanex.Data;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.NAP.Core.Command;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.MailDelivery.MailStation.Services
{
    public class MailGroupService : MailStationBaseService
    {
        public MailGroupService()
        {
        }

        public List<MailGroupModel> GetMailGroups(GetMailGroupCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
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

        public MailGroupModel GetMailGroupById(GetMailGroupByIdCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailGroupModel>().FirstOrDefault();
            }
        }
    }
}
