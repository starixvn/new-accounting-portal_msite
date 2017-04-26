using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Fanex.Letstalk.Notification.Services
{
    public class HttpClientService : IHttpClientService
    {
        private IAppConfigurationService service;

        public HttpClientService() : this(null)
        {

        }

        public HttpClientService(IAppConfigurationService service)
        {
            this.service = service ?? new AppConfigurationService();
        }

        public T PostAsync<T, P>(string action, P inputParams)
        {
            using (HttpClient client = new HttpClient())
            {
                string letstalkApiUrl = $"{service.LetstalkApiUrl}/Api/Letstalk/";
                client.BaseAddress = new Uri(letstalkApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(inputParams), Encoding.UTF8, "application/json");

                var responseResult = client.PostAsync(action, content).Result;

                if (responseResult.IsSuccessStatusCode)
                {
                    var data = responseResult.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(data.Result);
                }
            }

            return default(T);
        }
    }
}
