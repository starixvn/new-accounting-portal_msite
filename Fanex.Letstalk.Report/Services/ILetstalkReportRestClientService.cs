using Fanex.Letstalk.Report.Model;
using Fanex.Letstalk.Report.Params;
using System.Collections.Generic;

namespace Fanex.Letstalk.Report.Services
{
    public interface ILetstalkReportRestClientService
    {
        List<LetstalkRecord> GetLogs(GetLogParams queryParams);

        bool ReSendMail(ResendParams resendParams);

        bool ReSendMessage(ResendParams resendParams);
    }
}
