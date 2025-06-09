using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App_OficinaVirtual.Helpers
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        static HttpClientHelper()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            var token = Preferences.Get("access_token", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public static HttpClient GetClient() => _httpClient;

        public static void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}