using Fanex.Data;
using Fanex.Maintenance.Repository.Criteria.MailService;
using Fanex.Maintenance.Repository.Interface.Mail;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Maintenance.Repository.Impl.Mail
{
    using Data.Repository;

    public class MailRepository : IMailRepository
    {
        private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

        public IEnumerable<MailConfigLogModel> GetMailConfigLogs(int id)
        {
            var criteria = new GetMailConfigLogsCriteria { AppID = id };
            return dynamicRepository.Fetch<MailConfigLogModel>(criteria).ToList();
        }
    }
}
