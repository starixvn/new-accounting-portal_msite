namespace Fanex.Maintenance.Service.Mail
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.MailService;
    using Data.Repository;

    public interface IMailRepository
    {
        IEnumerable<MailConfigLogModel> GetMailConfigLogs(int id);
    }

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
