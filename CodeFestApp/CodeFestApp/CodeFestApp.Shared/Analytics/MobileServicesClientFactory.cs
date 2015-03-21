using System;
using System.Net;
using System.Net.Http;

namespace CodeFestApp.Analytics
{
    public class MobileServicesClientFactory
    {
        public HttpClient Create()
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(string.Empty, MobileServicesSettings.Key) };
            return new HttpClient(handler)
                {
                    BaseAddress = new Uri(MobileServicesSettings.Url)
                };
        }
    }
}
