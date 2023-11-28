using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Data.Repository;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStation.Constants;
using Fanex.MailDelivery.MailStation.Criteria;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.MailDelivery.MailStation.Criteria.MailReport;
using Fanex.MailDelivery.MailStation.Models;

namespace Fanex.MailDelivery.MailStation.Services
{
    using Maintenance.Service.Command;

    public class MailStationBaseService
    {
        private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

        public List<SelectListItem> Sites(bool hasAllElement = true)
        {
            var criteria = new GetAllSiteCriteria();
            var result = dynamicRepository.Fetch<SelectListItem>(criteria).ToList();
            
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(result).ToList();
            }

            return result;
        }

        public List<SelectListItem> MailGroups(bool hasAllElement = true)
        {
            var criteria = new GetAllMailGroupCriteria();
            var result = dynamicRepository.Fetch<SelectListItem>(criteria).ToList();

            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(result).ToList();
            }

            return result;
        }

        public List<SelectListItemExtra> Status(bool hasAllElement = true)
        {
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(MailStationConstants.Active).ToList();
            }

            return MailStationConstants.Active.ToList();
        }

        public List<SelectListItemExtra> TaskStatus(bool hasAllElement = true)
        {
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(MailStationConstants.TaskStatus).ToList();
            }

            return MailStationConstants.TaskStatus;
        }

        public List<SelectListItemExtra> Trigger(bool hasAllElement = true)
        {
            if (hasAllElement)
            {
                return MailStationConstants.AllItem.Union(MailStationConstants.TriggerTypes).ToList();
            }

            return MailStationConstants.TriggerTypes;
        }

        [AllowAnonymous]
        public List<SuggestMailModel> SuggestMails(SuggestMailCriteria criteria)
        {
            return dynamicRepository.Fetch<SuggestMailModel>(criteria).ToList();
        }

        public CommandResult ExecuteNonQuery(UpdateCommandBase command)
        {
            dynamicRepository.Execute(command);
            return new CommandResult(command.ErrCode, command.ErrMsg);
        }

        public MailTaskModel GetMailTaskByID(GetMailTaskByIdCriteria criteria)
        {
            return dynamicRepository.Get<MailTaskModel>(criteria);
        }

        public Schedule GetScheduleByMailId(GetScheduleByIdCriteria criteria)
        {
            return dynamicRepository.Get<Schedule>(criteria);
        }

        public string GetTimeZone()
        {
            return string.Empty;
        }
    }
}
