using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Fanex.Letstalk.Report.Services
{
    public class HttpClientService : IHttpClientService
    {
        public const string letstalkApiUrl = "http://ha.nexdev.net:7309/Api/Letstalk/";

        public T PostAsync<T, P>(string action, P inputParams)
        {
            using (HttpClient client = new HttpClient())
            {
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
