using System;
using System.Data;
using System.Data.SqlClient;
using StockImport.Domain;

namespace StockImport.Storage
{
    class ShareRepository : IShareRepository 
    {
        private readonly SqlConnection connection;
        private readonly string tableName;

        public ShareRepository(string connectionString, string tableName)
        {
            this.tableName = tableName;
            this.connection = new SqlConnection(connectionString);
            this.connection.Open();
        }

        public void Save(Share share)
        {
            var data = AsDataTable(share);
            this.BulkInsertStock(data);
        }

        private void BulkInsertStock(DataTable stock)
        {
            var transaction = this.connection.BeginTransaction();

            using (var bulkCopy = new SqlBulkCopy(this.connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = this.tableName;
                try
                {
                    bulkCopy.WriteToServer(stock);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            transaction.Commit();
        }

        private static DataTable AsDataTable(Share share)
        {
            var table = new DataTable();
            AddQuoteColumns(table);
            AddCalculatedColumns(table, share);
            foreach (var pq in share.ProcessedQuotes)
            {
                DataRow row = table.NewRow();
                AddQuoteRowData(share.Id, row, pq);
                AddCalculatedRowData(row, pq);
                table.Rows.Add(row);
            }
            return table;
        }

        private static void AddQuoteColumns(DataTable table)
        {
            table.Columns.Add("StockId", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Open", typeof(Decimal));
            table.Columns.Add("High", typeof(Decimal));
            table.Columns.Add("Low", typeof(Decimal));
            table.Columns.Add("Close", typeof(Decimal));
            table.Columns.Add("Volume", typeof(Decimal));
            table.Columns.Add("AdjClose", typeof(Decimal));
        }

        private static void AddCalculatedColumns(DataTable table, Share share)
        {
            foreach(var calculation in share.Calculations)
            {
                table.Columns.Add(calculation.ToString(), typeof(Decimal));
            }
        }

        private static void AddQuoteRowData(string shareId, DataRow row, ProcessedQuote pq)
        {
            row["StockId"] = shareId;
            row["Date"] = pq.Quote.Date;
            row["Open"] = pq.Quote.Open;
            row["High"] = pq.Quote.High;
            row["Low"] = pq.Quote.Low;
            row["Close"] = pq.Quote.Close;
            row["Volume"] = pq.Quote.Volume;
            row["AdjClose"] = pq.Quote.AdjClose;
        }

        private static void AddCalculatedRowData(DataRow row, ProcessedQuote pq)
        {
            foreach (var calculation in pq)
            {
                row[calculation.Key.ToString()] = calculation.Value;
            }
            
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.connection.Close();
                this.connection.Dispose();
            }
        }
    }
}