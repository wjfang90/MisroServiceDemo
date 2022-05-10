using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Framework.Utility
{
    public class Utility : IUtility
    {
        private HttpClient _httpClient;
        public Utility(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string GetResponse(string url)
        {
            var uri = new Uri(url);
            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = HttpMethod.Get
            };
            var resopnse = _httpClient.SendAsync(request).Result;
            if (resopnse.IsSuccessStatusCode)
            {
                return resopnse.Content.ReadAsStringAsync().Result;
            }
            return null;
        }
    }

    public interface IUtility
    {
        string GetResponse(string url);
    }
}
