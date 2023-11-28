namespace Fanex.Maintenance.Service.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MailDeliver.MailClient.API;
    using MailDeliver.Model;

    public interface IMailService
    {
        List<MailConfig> GetListConfig(string ip = "");

        IMailService WithApp(int appId);

        IMailService WithStatus(int status);

        IMailService FromDate(string date);

        IMailService ToDate(string date);

        IMailService WithPagination(int page, int counter);

        List<MailRecord> GetListMail();

        List<MailRecord> GetListMailByDate(ref int totalRows);

        MailRecord GetMail(int mailId);

        MailSetting GetSetting(int settingId);

        MailConfig GetConfig(int configId);

        MailFunction GetFunction(int functionId);

        List<MailSetting> GetListSetting();

        List<MailFunction> GetListFunction();

        bool SendMail(long mailID);

        bool StoreSetting(MailSetting setting);

        bool StoreConfig(MailConfig config);

        bool StoreFunction(MailFunction func);

        List<MailError> GetError(int mailId);
    }

    public class MailService : IMailService
    {
        private int _appId;

        private string _fromDate;

        private string _toDate;

        private int _page;

        private int _counter;

        private MailStatus _mailStatus;

        private MailService()
        {
        }

        public static MailService Create()
        {
            return new MailService();
        }

        public List<MailConfig> GetListConfig(string ip = "")
        {
            var configs = new List<MailConfig>();

            using (var client = new MailDeliver())
            {
                var data = client.GetListConfig(ip);
                if (data != null)
                {
                    configs = data.OrderBy(x => x.AppName).ToList();
                }
            }

            return configs;
        }

        public IMailService WithApp(int appId)
        {
            this._appId = appId;

            return this;
        }

        public IMailService FromDate(string date)
        {
            this._fromDate = date;

            return this;
        }

        public IMailService ToDate(string date)
        {
            this._toDate = date;

            return this;
        }

        public IMailService WithPagination(int page, int counter)
        {
            this._page = page;
            this._counter = counter;

            return this;
        }

        public List<MailRecord> GetListMail()
        {
            var mails = new List<MailRecord>();

            using (var client = new MailDeliver())
            {
                var dateTime = DateTime.Parse(_fromDate);
                var data = client.GetListMail(_appId, dateTime, _page, _counter);
                if (data != null)
                {
                    mails = data.ToList();
                }
            }

            return mails;
        }

        public List<MailRecord> GetListMailByDate(ref int totalRows)
        {
            var mails = new List<MailRecord>();

            using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
            {
                var dateTime = DateTime.Parse(_fromDate);
                var dateTimeTo = DateTime.Parse(_toDate);
                var data = client.GetListMailByDate(_appId, dateTime, dateTimeTo, ref totalRows, _page, _counter, _mailStatus);

                if (data != null)
                {
                    mails = data.Select(mailRecord =>
                    {
                        mailRecord.MailTo = mailRecord.MailTo.Replace(";", "<br />");
                        mailRecord.MailBcc = mailRecord.MailBcc.Replace(";", "<br />");
                        mailRecord.MailCc = mailRecord.MailCc.Replace(";", "<br />");

                        return mailRecord;
                    }).ToList();
                }
            }

            return mails;
        }

        public MailRecord GetMail(int mailID)
        {
            MailRecord mail = null;

            using (var client = new MailDeliver())
            {
                mail = client.GetMail(mailID);
            }

            return mail;
        }

        public List<MailSetting> GetListSetting()
        {
            var settings = new List<MailSetting>();

            using (var client = new MailDeliver())
            {
                var data = client.GetListSetting();
                if (data != null)
                {
                    settings = data.OrderBy(x => x.Server).ToList();
                }
            }

            return settings;
        }

        public List<MailFunction> GetListFunction()
        {
            var functions = new List<MailFunction>();

            using (var client = new MailDeliver())
            {
                var data = client.GetListFuntion();
                if (data != null)
                {
                    functions = data.ToList();
                }
            }

            return functions;
        }

        public bool SendMail(long mailID)
        {
            try
            {
                using (var client = new MailDeliver())
                {
                    client.ReSendMail(mailID);
                }

                return true;
            }
            catch
            {
            }

            return false;
        }

        public bool StoreSetting(MailSetting setting)
        {
            var result = false;

            if (setting != null)
            {
                using (var client = new MailDeliver())
                {
                    client.StoreSetting(setting);
                    result = true;
                }
            }

            return result;
        }

        public bool StoreConfig(MailConfig config)
        {
            var result = false;

            if (config != null)
            {
                using (var client = new MailDeliver())
                {
                    client.StoreConfig(config);
                    result = true;
                }
            }

            return result;
        }

        public bool StoreFunction(MailFunction func)
        {
            var result = false;

            if (func != null)
            {
                using (var client = new MailDeliver())
                {
                    client.StoreFunction(func);
                    result = true;
                }
            }

            return result;
        }

        public MailSetting GetSetting(int settingId)
        {
            MailSetting mail = null;

            using (var client = new MailDeliver())
            {
                mail = client.GetSetting(settingId);
            }

            return mail;
        }

        public MailConfig GetConfig(int configId)
        {
            MailConfig mail = null;

            using (var client = new MailDeliver())
            {
                mail = client.GetConfig(configId);
            }

            return mail;
        }

        public MailFunction GetFunction(int functionId)
        {
            MailFunction mail = null;

            using (var client = new MailDeliver())
            {
                mail = client.GetFunction(functionId);
            }

            return mail;
        }

        public IMailService WithStatus(int status)
        {
            if (status == 0)
            {
                this._mailStatus = MailStatus.All;
            }

            if (status == -1)
            {
                this._mailStatus = MailStatus.Fail;
            }

            if (status == 1)
            {
                this._mailStatus = MailStatus.Success;
            }

            return this;
        }

        public List<MailError> GetError(int mailId)
        {
            var mailList = new List<MailError>();

            using (var client = new MailDeliver())
            {
                mailList = client.GetListError(mailId).ToList();
            }

            return mailList;
        }
    }
}
