using Fanex.Maintenance.Repository.Criteria.MailService;
using System.Collections.Generic;

namespace Fanex.Maintenance.Repository.Interface.Mail
{
    public interface IMailRepository
    {
        IEnumerable<MailConfigLogModel> GetMailConfigLogs(int id);
    }
}
