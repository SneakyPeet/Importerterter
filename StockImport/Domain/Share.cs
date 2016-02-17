using System.Collections.Generic;
using System.Linq;
using StockImport.Calculators;

namespace StockImport.Domain
{
    public class Share
    {
        public readonly List<ProcessedQuote> ProcessedQuotes;
        public string Id { get; private set; }
        public IReadOnlyList<Calculation> Calculations { get; private set; }

        public Share(string id, IEnumerable<IQuote> quotes, ProcessEngine engine)
        {
            this.Id = id;

            this.ProcessedQuotes = 
                quotes
                .Clean()
                .Order()
                .ToProcessableStock();

            this.Calculations = engine.Keys.ToList().AsReadOnly();

            this.Process(engine);
        }

        

        private void Process(ProcessEngine engine)
        {
            var sma12 = new SmaCalculator(12);
            foreach(var quote in this.ProcessedQuotes)
            {
                engine.Process(quote);
            }
        }
    }
}