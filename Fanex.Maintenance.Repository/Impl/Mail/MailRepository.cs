using Fanex.Data;
using Fanex.Maintenance.Repository.Criteria.MailService;
using Fanex.Maintenance.Repository.Interface.Mail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Maintenance.Repository.Impl.Mail
{
    public class MailRepository : IMailRepository
    {
        public IEnumerable<MailConfigLogModel> GetMailConfigLogs(int id)
        {
            List<MailConfigLogModel> result;
            var criteria = new GetMailConfigLogsCriteria { AppID = id };
            var param = criteria.GetSpParams();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<MailConfigLogModel>(param).ToList();
            }
            //result = new List<MailConfigLogModel>
            //{
            //    new MailConfigLogModel { AppID = id, AppName = "Aff_Livebet365", Description= "Affiliate Live bet 365", Setting = "smtp.nexdev.net:25 (warehouse@starixsoft.com)", IsEncrypt = false, LogTime = DateTime.Now, MailFrom = "warehouse@starixvn.com", UserID = 1, Username = "sa" },
            //    new MailConfigLogModel { AppID = id, AppName = "Aff_Livebet365", Description= "Affiliate Live bet 365", Setting = "smtp.nexdev.net:25 (warehouse@starixsoft.com)", IsEncrypt = false, LogTime = DateTime.Now, MailFrom = "warehouse@starixvn.com", UserID = 1, Username = "sa" },
            //};
            return result;
        }
    }
}
