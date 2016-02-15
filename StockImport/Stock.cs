using System;
using FileHelpers;

namespace StockImport
{
    //properties needs to be in the same order as in the file
    [DelimitedRecord(",")]
    [IgnoreFirst]
    [IgnoreEmptyLines]
    public class Stock
    {
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime Date;
        public decimal Open;
        public decimal High;
        public decimal Low;
        public decimal Close;
        public decimal Volume;
        public decimal Average;

        [FieldOptional]
        public string StockId;

    }
}