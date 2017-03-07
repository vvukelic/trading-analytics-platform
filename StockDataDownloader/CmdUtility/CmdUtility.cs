using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using IntrinioDownloaderLib;

namespace CmdUtility
{
    class CmdUtility
    {
        private LiveStockData liveStockData;
        private List<string> tickers = new List<string>();

        public CmdUtility()
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            liveStockData = new LiveStockData(username, password);
        }

        public void Run()
        {
            while (true)
            {
                Console.Write("> ");

                string action = Console.ReadLine();

                switch (action)
                {
                    case "h":
                        PrintHelp();
                        break;

                    case "g":
                        GetStockData();
                        break;

                    case "d":
                        break;

                    case "c":
                        break;

                    case "q":
                        return;
                }
            }
        }

        private void GetStockData()
        {
            if (tickers.Count == 0)
            {
                Console.WriteLine("No tickers defined.");
                return;
            }

            try
            {
                List<StockPriceInfo> stockPriceInfos = liveStockData.GetLatestPrice(tickers);

                foreach (StockPriceInfo priceInfo in stockPriceInfos)
                {
                    Console.WriteLine($"{priceInfo.Identifier}: {priceInfo.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("h - help\ng - get stock data\nd - define tickers\nc - clear tickers\nq - quit");
        }
    }
}
