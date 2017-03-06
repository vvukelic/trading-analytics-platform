using RestSharp;
using RestSharp.Authenticators;

namespace IntrinioDownloaderLib
{
    public class LiveStockData
    {
        private const string baseUrl = "https://api.intrinio.com";
        private RestClient client;

        public LiveStockData(string username, string password)
        {
            client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        public float GetLatestPrice(string ticker)
        {
            RestRequest request = new RestRequest("data_point?identifier={ticker}&item=last_price", Method.GET);

            request.AddUrlSegment("ticker", ticker);

            Data response = client.Execute<Data>(request).Data;

            return response.Value;
        }
    }

    internal class Data
    {
        public string Identifier { get; set; }
        public string Item { get; set; }
        public float Value { get; set; }
    }
}
