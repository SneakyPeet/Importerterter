using System;
using FileHelpers;

namespace StockImport
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    [IgnoreEmptyLines]
    public class Quote
    {
        [FieldOrder(1)]
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime Date;

        [FieldOrder(2)]
        public decimal Open;

        [FieldOrder(3)]
        public decimal High;

        [FieldOrder(4)]
        public decimal Low;

        [FieldOrder(5)]
        public decimal Close;

        [FieldOrder(6)]
        public decimal Volume;

        [FieldOrder(7)]
        public decimal AdjClose;
    }
}