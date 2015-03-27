using System;
using System.Net.Http;

using Microsoft.WindowsAzure.MobileServices;

namespace CodeFestApp.Analytics
{
    public class MobileServicesClientFactory
    {
        public HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(MobileServicesSettings.Url) };

            // https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx
            httpClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", MobileServicesSettings.Key);

            return httpClient;
        }

        public IMobileServiceClient Create()
        {
            return new MobileServiceClient(MobileServicesSettings.Url, MobileServicesSettings.Key);
        }
    }
}
