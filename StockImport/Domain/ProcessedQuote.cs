﻿using System;
using System.Collections.Generic;

namespace StockImport.Domain
{
    public class ProcessedQuote : Dictionary<Calculation, Decimal>
    {
        public ProcessedQuote(Quote stock)
        {
            this.Quote = stock;
        }

        public Quote Quote { get; private set; }
    }
}