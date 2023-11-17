using System.Collections.Generic;
using System.Linq;
using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.Maintenance.Repository.Command;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Service.Interface;

namespace Fanex.MailDelivery.MailStation.Services.ManageRecipient
{
    using Data.Repository;
    using NAP.Core.Service.Impl;

    public class ManageRecipientService : MailStationBaseService
    {
        private readonly IUserService userService = new UserService();

        private readonly IDynamicRepository dynamicRepository= new DynamicRepository();

        public List<RecipientModel> RecipientList(RecipientListCriteria criteria)
        {
            return dynamicRepository.Fetch<RecipientModel>(criteria).ToList();
        }

        public RecipientModel RecipientById(RecipientByIdCriteria criteria)
        {
            return dynamicRepository.Get<RecipientModel>(criteria);
        }

        public CommandResult UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            return new CommandResult(criteria.ErrCode, criteria.ErrMsg);
        }

        public CommandResult CreateRecipient(CreateRecipientCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            return new CommandResult(criteria.ErrCode, criteria.ErrMsg);
        }

        public CommandResult DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            return new CommandResult(criteria.ErrCode, criteria.ErrMsg);
        }

        public User CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            return userService.Get(criteria.Username);
        }
    }
}