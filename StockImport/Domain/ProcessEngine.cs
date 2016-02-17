using System.Collections.Generic;

namespace StockImport.Domain
{
    public class ProcessEngine : Dictionary<Calculation, ICalculator>
    {
        public void Process(ProcessedQuote quote)
        {
            foreach (var calculator in this)
            {
                quote.Add(calculator.Key, calculator.Value.NextValue(quote.Quote.Close));
            }
               
        }

        public void Reset()
        {
            foreach(var calculator in this.Values)
            {
                calculator.Reset();
            }
        }
    }
}