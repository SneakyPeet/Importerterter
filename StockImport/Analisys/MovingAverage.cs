using System;
using System.Collections.Generic;
using System.Linq;

namespace StockImport.Analisys
{
    public static class MovingAverageExctensions
    {
        //assumes stocks are ordered
        private static IEnumerable<Stock> Sma(this List<Stock> stocks, int days, Func<Stock, decimal, Nothing> set)
        {
            if(stocks.Any())
            {
                var firstPrice = stocks.First().Close;
                var queue = new FixedSizedQueue(days, firstPrice);
                foreach(var stock in stocks)
                {
                    queue.Enqueue(stock.Close);
                    set(stock, queue.Sum()/days);
                }
            }
            return stocks;
        }


        public static IEnumerable<Stock> Sma12(this List<Stock> stocks)
        {
            Func<Stock, decimal, Nothing> set = (stock, value) => { 
                stock.Sma12 = value;
                return Nothing.Value();
            };
            return stocks.Sma(12, set);
        }
    }
}