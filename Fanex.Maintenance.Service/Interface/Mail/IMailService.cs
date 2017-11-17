namespace Fanex.Maintenance.Service.Interface.Mail
{
    using System.Collections.Generic;
    using Fanex.MailDeliver.Model;

    /// <summary>
    /// Mail service interface.
    /// </summary>
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
}