using System.Collections.Generic;
using System.Linq;

using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

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

        public StockPriceInfo GetLatestPrice(string ticker)
        {
            RestRequest request = new RestRequest("data_point?identifier={tickerr}&item=last_price", Method.GET);

            request.AddUrlSegment("ticker", ticker);

            StockPriceInfo response = client.Execute<StockPriceInfo>(request).Data;

            return response;
        }

        public List<StockPriceInfo> GetLatestPrice(List<string> tickers)
        {
            RestRequest request = new RestRequest("data_point?identifier={tickers}&item=last_price", Method.GET);

            string tickers_string = string.Empty;

            foreach (string ticker in tickers)
            {
                tickers_string += $"{ticker},";
            }

            request.AddUrlSegment("tickers", tickers_string);

            StockPriceInfoMultiple response = client.Execute<StockPriceInfoMultiple>(request).Data;

            return response.Data;
        }
    }

    public struct StockPriceInfo
    {
        public string Identifier { get; set; }
        public string Item { get; set; }
        public float Value { get; set; }
    }

    internal struct StockPriceInfoMultiple
    {
        public List<StockPriceInfo> Data { get; set; }

        [DeserializeAs(Name = "result_count")]
        public int ResultCount { get; set; }

        [DeserializeAs(Name = "api_call_credits")]
        public int ApiCallCredits { get; set; }
    }
}
