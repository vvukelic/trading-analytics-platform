using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;

using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;

using IntrinioDownloaderLib;

namespace StockDataDownloader
{
    class StockDataDownloader
    {
        private LiveStockData liveStockData;
        private List<string> tickers = new List<string>();
        private InfluxDbClient dbClient;

        public StockDataDownloader()
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            liveStockData = new LiveStockData(username, password);

            tickers.AddRange(ConfigurationManager.AppSettings["tickers"].Split(new char[] { ' ' }));

            dbClient = new InfluxDbClient("http://localhost:8086", "root", "root", InfluxData.Net.Common.Enums.InfluxDbVersion.Latest);
        }

        public void Run()
        {
            Console.WriteLine("Starting stoc data downloader...");

            while (true)
            {
                List<StockPriceInfo> prices = liveStockData.GetLatestPrice(tickers.AsReadOnly());

                WritePricesToDb(prices);

                Thread.Sleep(1000 * 30);
            }
        }

        private async void WritePricesToDb(List<StockPriceInfo> prices)
        {
            List<Point> pointsToWrite = new List<Point>();

            foreach (StockPriceInfo stockPriceInfo in prices)
            {
                Point point = new Point()
                {
                    Name = "prices",
                    Tags = new Dictionary<string, object>()
                    {
                        { "symbol", stockPriceInfo.Identifier }
                    },
                    Fields = new Dictionary<string, object>()
                    {
                        { "price", stockPriceInfo.Value }
                    },
                    Timestamp = DateTime.UtcNow
                };

                pointsToWrite.Add(point);
            }

            var response = await dbClient.Client.WriteAsync("stocks", pointsToWrite);
            
            if (response.Success)
            {
                Console.WriteLine($"Succesfully written to Db at {DateTime.UtcNow}");
            }
        }
    }
}
