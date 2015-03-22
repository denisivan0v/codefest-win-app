using System;
using System.Net.Http;

namespace CodeFestApp.Analytics
{
    public class MobileServicesClientFactory
    {
        public HttpClient Create()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(MobileServicesSettings.Url) };

            // https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx
            httpClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", MobileServicesSettings.Key);

            return httpClient;
        }
    }
}
