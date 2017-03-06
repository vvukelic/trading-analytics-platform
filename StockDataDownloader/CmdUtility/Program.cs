using System;
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

            float lastPrice = liveStockData.GetLatestPrice("AMD");

            Console.WriteLine($"{lastPrice}");
        }
    }
}
