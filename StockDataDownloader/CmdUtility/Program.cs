using System;
using System.Collections.Generic;
using System.Configuration;

using IntrinioDownloaderLib;

namespace CmdUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            LiveStockData liveStockData = new LiveStockData(username, password);

            List<string> tickers = new List<string> { "AMD", "AAPL", "NBR" };

            List<StockPriceInfo> stockPriceInfos = liveStockData.GetLatestPrice(tickers);

            foreach (StockPriceInfo priceInfo in stockPriceInfos)
            {
                Console.WriteLine($"{priceInfo.Identifier}: {priceInfo.Value}");
            }
        }
    }
}
