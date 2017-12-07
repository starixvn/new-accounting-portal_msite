using System.Collections.Generic;
using Fanex.Letstalk.Notification.Model;
using Fanex.Letstalk.Notification.Params;

namespace Fanex.Letstalk.Notification.Services
{
    public class LetstalkReportRestClientService : ILetstalkReportRestClientService
    {
        private readonly IHttpClientService clientService;

        public LetstalkReportRestClientService() : this(null)
        {
        }

        public LetstalkReportRestClientService(IHttpClientService clientService)
        {
            this.clientService = clientService ?? new HttpClientService();
        }

        public List<FilterTarget> GetTargets(GetLogParams queryParams)
        {
            var result = clientService.PostAsync<List<FilterTarget>, GetLogParams>("GetTargets", queryParams);
            return result;
        }
        public List<FilterGroup> GetGroupsByTarget(GetLogParams queryParams)
        {
            var result = clientService.PostAsync<List<FilterGroup>, GetLogParams>("GetGroupsByTarget", queryParams);
            return result;
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