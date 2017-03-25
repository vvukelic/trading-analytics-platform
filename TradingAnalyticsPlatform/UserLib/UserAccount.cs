using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLib
{
    public class UserAccount
    {
        public List<string> Tickers { get { return tickers; } }

        private List<string> tickers = new List<string>();

        public void AddTicker(string ticker)
        {
            tickers.Add(ticker);
        }

        public void AddTickers(string[] ticker_array)
        {
            tickers.AddRange(ticker_array);
        }

        public void RemoveTicker(string ticker)
        {
            tickers.Remove(ticker);
        }

        public void ClearTickers()
        {
            tickers.Clear();
        }
    }
}
