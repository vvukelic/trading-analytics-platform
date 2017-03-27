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
using UserLib;

namespace StockDataDownloader
{
    class StockDataDownloader
    {
        private LiveStockData liveStockData;
        private UserAccount userAccount;
        private InfluxDbClient dbClient;

        public StockDataDownloader()
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            liveStockData = new LiveStockData(username, password);

            userAccount = new UserAccount();
            userAccount.AddTickers(ConfigurationManager.AppSettings["tickers"].Split(new char[] { ' ' }));

            dbClient = new InfluxDbClient("http://localhost:8086", "root", "root", InfluxData.Net.Common.Enums.InfluxDbVersion.Latest);
        }

        public void Run()
        {
            List<StockPriceInfo> prices = liveStockData.GetLatestPrice(userAccount.Tickers);

            WritePricesToDb(prices);

            Thread.Sleep(5000);
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
                Console.WriteLine("Succesfully written to Db!");
            }
        }
    }
}
