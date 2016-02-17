using System;
using System.Collections.Generic;

namespace StockImport.Domain
{
    public class ProcessedQuote : Dictionary<Calculation, Decimal>
    {
        public ProcessedQuote(IQuote stock)
        {
            this.Quote = stock;
        }

        public IQuote Quote { get; private set; }
    }
}