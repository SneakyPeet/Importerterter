using System.Collections.Generic;
using StockImport.Analisys;

namespace StockImport
{
    public class Share
    {
        public readonly List<ProcessedQuote> ProcessedQuotes;
        public string Id { get; private set; } 

        public Share(string filePath)
        {
            this.Id = filePath.ToShareId();

            this.ProcessedQuotes = 
                filePath
                .ReadFile()
                .Clean()
                .Order()
                .ToProcessableStock();

            Process();
        }

        private void Process()
        {
            var sma12 = new MovingAverageCalculator(12);
            foreach(var stock in this.ProcessedQuotes)
            {
                stock.Add(Calculation.Sma12, sma12.NextValue(stock.Quote.Close));
            }
        }
    }
}