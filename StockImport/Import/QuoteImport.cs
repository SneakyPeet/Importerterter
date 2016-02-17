using System;
using FileHelpers;
using StockImport.Domain;

namespace StockImport.Import
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    [IgnoreEmptyLines]
    public class QuoteImport : IQuote
    {
        [FieldOrder(1)]
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime date;

        [FieldOrder(2)]
        public decimal open;

        [FieldOrder(3)]
        public decimal high;

        [FieldOrder(4)]
        public decimal low;

        [FieldOrder(5)]
        public decimal close;

        [FieldOrder(6)]
        public decimal volume;

        [FieldOrder(7)]
        public decimal adjClose;

        public DateTime Date { get { return date; }  }
        public decimal Open { get { return open; } }
        public decimal High { get { return high; } }
        public decimal Low { get { return low; } }
        public decimal Close { get { return close; } }
        public decimal Volume { get { return volume; } }
        public decimal AdjClose { get { return adjClose; } }
    }
}