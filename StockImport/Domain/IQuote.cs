using System;

namespace StockImport.Domain
{
    public interface IQuote 
    {
        DateTime Date { get; }
        decimal Open { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Close { get; }
        decimal Volume { get; }
        decimal AdjClose { get; }
    }
}