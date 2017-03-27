using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            StockDataDownloader stockDataDownloader = new StockDataDownloader();

            stockDataDownloader.Run();
        }

        /*static async void GetResponse(InfluxDbClient client)
        {
            var query = "SELECT * FROM stock WHERE symbol = 'AAPL'";
            var response = await client.Client.QueryAsync("testDb", query);

            foreach (var r in response)
            {
                foreach (var v in r.Values)
                {
                    Console.WriteLine($"{v[0]} {v[1]} {v[2]}");
                }
            }
        }*/
    }
}
