using System;
using FileHelpers;

namespace StockImport
{
    [DelimitedRecord(";")]
    [IgnoreFirst]
    [IgnoreEmptyLines]
    public class Stock
    {
        private Stock(){ } //required by filehelpers

        public Stock(StockImport stock, String stockId)
        {
            this.Date = stock.Date;
            this.Open = stock.Open;
            this.High = stock.High;
            this.Low = stock.Low;
            this.Close = stock.Close;
            this.Volume = stock.Volume;
            this.AdjClose = stock.AdjClose;
            this.StockId = stockId;
        }

        [FieldOrder(1)]
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime Date;

        [FieldOrder(2)]
        [FieldConverter(ConverterKind.Decimal, ",")]
        public decimal Open;

        [FieldOrder(3)]
        [FieldConverter(ConverterKind.Decimal, ",")]
        public decimal High;

        [FieldOrder(4)]
        [FieldConverter(ConverterKind.Decimal, ",")]
        public decimal Low;

        [FieldOrder(5)]
        [FieldConverter(ConverterKind.Decimal, ",")]
        public decimal Close;

        [FieldOrder(6)]
        [FieldConverter(ConverterKind.Decimal, ",")]
        public decimal Volume;

        [FieldOrder(7)]
        [FieldConverter(ConverterKind.Decimal, ",")]
        public decimal AdjClose;

        [FieldOrder(8)]
        public string StockId;
    }
}