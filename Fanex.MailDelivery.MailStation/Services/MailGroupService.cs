using System.Collections.Generic;
using System.Linq;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Models;

namespace Fanex.MailDelivery.MailStation.Services
{
    using Data.Repository;
    using Maintenance.Service.Command;

    public class MailGroupService : MailStationBaseService
    {
      private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

        public List<MailGroupModel> GetMailGroups(GetMailGroupCriteria criteria)
        {
            return dynamicRepository.Fetch<MailGroupModel>(criteria).ToList();
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
            return dynamicRepository.Get<MailGroupModel>(criteria);
        }
    }
}
