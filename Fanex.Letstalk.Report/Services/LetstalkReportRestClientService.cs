using System.Collections.Generic;
using Fanex.Letstalk.Notification.Model;
using Fanex.Letstalk.Notification.Params;

namespace Fanex.Letstalk.Notification.Services
{
    public class LetstalkReportRestClientService : ILetstalkReportRestClientService
    {
        private IHttpClientService clientService;

        public LetstalkReportRestClientService() : this(null)
        {
        }

        public LetstalkReportRestClientService(IHttpClientService clientService)
        {
            this.clientService = clientService ?? new HttpClientService();
        }

        public List<LetstalkRecord> GetLogs(GetLogParams queryParams)
        {
            var result = clientService.PostAsync<List<LetstalkRecord>, GetLogParams>("GetLogs", queryParams);
            return result;
        }

        public bool ReSendMail(ResendParams resendParams)
        {
            var result = clientService.PostAsync<bool, ResendParams>("ReSendMail", resendParams);
            return result;
        }

        public bool ReSendMessage(ResendParams resendParams)
        {
            var result = clientService.PostAsync<bool, ResendParams>("ReSendMessage", resendParams);
            return result;
        }
    }
}