using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using IntrinioDownloaderLib;
using UserLib;

namespace CmdUtility
{
    class CmdUtility
    {
        private LiveStockData liveStockData;
        private UserAccount userAccount;

        public CmdUtility()
        {
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];

            liveStockData = new LiveStockData(username, password);

            userAccount = new UserAccount();
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

                    case "a":
                        AddTickers();
                        break;

                    case "p":
                        PrintTickerList();
                        break;

                    case "d":
                        DeleteTickers();
                        break;

                    case "c":
                        ClearTickers();
                        break;

                    case "q":
                        return;
                }
            }
        }

        private void GetStockData()
        {
            if (userAccount.Tickers.Count == 0)
            {
                Console.WriteLine("No tickers defined.");
                return;
            }

            try
            {
                List<StockPriceInfo> stockPriceInfos = liveStockData.GetLatestPrice(userAccount.Tickers);

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

        private void AddTickers()
        {
            Console.WriteLine("List tickers to add (delimiter = space):");
            string line = Console.ReadLine();

            userAccount.AddTickers(line.Split(new char[] { ' ' }));
        }

        private void PrintTickerList()
        {
            Console.WriteLine($"You have {userAccount.Tickers.Count} tickers:");

            foreach (string ticker in userAccount.Tickers)
            {
                Console.WriteLine(ticker);
            }
        }

        private void DeleteTickers()
        {
            Console.WriteLine($"Which tickers do you want to delete (delimiter = space):");
            string line = Console.ReadLine();

            string[] tickersToDelete = line.Split(new char[] { ' ' });

            foreach (string tickerToDelete in tickersToDelete)
            {
                userAccount.Tickers.Remove(tickerToDelete);
            }
        }

        private void ClearTickers()
        {
            userAccount.ClearTickers();
        }

        private void PrintHelp()
        {
            Console.WriteLine("h - help");
            Console.WriteLine("g - get stock data");
            Console.WriteLine("a - add tickers");
            Console.WriteLine("p - print tickers");
            Console.WriteLine("d - delete tickers");
            Console.WriteLine("c - clear tickers");
            Console.WriteLine("q - quit");
        }
    }
}
