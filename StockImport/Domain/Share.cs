using System.Collections.Generic;
using System.Linq;
using StockImport.Calculators;
using StockImport.Helpers;

namespace StockImport.Domain
{
    public class Share
    {
        public readonly List<ProcessedQuote> ProcessedQuotes;
        public string Id { get; private set; }
        public IReadOnlyList<Calculation> Calculations { get; private set; }

        public Share(string filePath, ProcessEngine engine)
        {
            this.Id = filePath.ToShareId();

            this.ProcessedQuotes = 
                filePath
                .ReadFile()
                .Clean()
                .Order()
                .ToProcessableStock();

            this.Calculations = engine.Keys.ToList().AsReadOnly();

            this.Process(engine);
        }

        

        private void Process(ProcessEngine engine)
        {
            var sma12 = new MovingAverageCalculator(12);
            foreach(var quote in this.ProcessedQuotes)
            {
                engine.Process(quote);
            }
        }
    }
}