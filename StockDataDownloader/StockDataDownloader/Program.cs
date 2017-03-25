using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using InfluxData.Net.InfluxDb;

namespace StockDataDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new InfluxDbClient("http://localhost:8086", "root", "root", InfluxData.Net.Common.Enums.InfluxDbVersion.Latest);

            GetResponse(client);

            Thread.Sleep(1000);
        }

        static async void GetResponse(InfluxDbClient client)
        {
            var query = "SELECT * FROM stock WHERE symbol = 'AAPL'";
            var response = await client.Client.QueryAsync("testDb", query);

            foreach (var r in response)
            {
                foreach (var v in r.Values)
                {
                    Console.WriteLine($"{v[0]} {v[1]} {v[2]} {v[3]}");
                }
            }
        }
    }
}
