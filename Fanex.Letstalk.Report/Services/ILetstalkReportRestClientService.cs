using Fanex.Letstalk.Notification.Model;
using Fanex.Letstalk.Notification.Params;
using System.Collections.Generic;

namespace Fanex.Letstalk.Notification.Services
{
    public interface ILetstalkReportRestClientService
    {
        List<LetstalkRecord> GetLogs(GetLogParams queryParams);

        bool ReSendMail(ResendParams resendParams);

        bool ReSendMessage(ResendParams resendParams);
    }
}
