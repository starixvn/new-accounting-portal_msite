using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Fanex.Letstalk.Notification.Services
{
    public interface IHttpClientService
    {
        T PostAsync<T, P>(string action, P inputParams);
    }

    public class HttpClientService : IHttpClientService
    {
        private readonly IAppConfigurationService service;

        private static readonly HttpClient Client = new HttpClient();

        public HttpClientService() : this(null)
        {
        }

        public HttpClientService(IAppConfigurationService service)
        {
            this.service = service ?? new AppConfigurationService();
        }

        public T PostAsync<T, P>(string action, P inputParams)
        {
            var letstalkApiUrl = $"{service.LetstalkApiUrl}Letstalk/";
            Client.BaseAddress = new Uri(letstalkApiUrl);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(inputParams), Encoding.UTF8, "application/json");

            var responseResult = Client.PostAsync(action, content).Result;

            if (responseResult.IsSuccessStatusCode)
            {
                var data = responseResult.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data.Result);
            }

            return default(T);
        }
    }
}
