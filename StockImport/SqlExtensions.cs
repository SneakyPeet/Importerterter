﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace StockImport
{
    public static class SqlExtensions
    {
        private const string connectionString = "Server=.;Database=Stocks;Trusted_Connection=True;";
        private const string tableName = "dbo.Stocks";

        public static int ImportToSql(this IEnumerable<string> files, Func<string, IEnumerable<Stock>> processFile)
        {
            var total = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach(var file in files)
                {
                    var stock = processFile(file).AsDataTable();
                    BulkInsertStock(connection, stock);
                    total += stock.Rows.Count;
                }
                connection.Close();
            }
            return total;
        }

        private static void BulkInsertStock(SqlConnection connection, DataTable stock)
        {
            var transaction = connection.BeginTransaction();

            using(var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = tableName;
                try
                {
                    bulkCopy.WriteToServer(stock);
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    connection.Close();
                    throw;
                }
            }
            transaction.Commit();
        }

        private static DataTable AsDataTable(this IEnumerable<Stock> data)
        {
            var table = new DataTable();
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Open", typeof(Decimal));
            table.Columns.Add("High", typeof(Decimal));
            table.Columns.Add("Low", typeof(Decimal));
            table.Columns.Add("Close", typeof(Decimal));
            table.Columns.Add("Volume", typeof(Decimal));
            table.Columns.Add("AdjClose", typeof(Decimal));
            table.Columns.Add("StockId", typeof(string));
            foreach (var item in data)
            {
                DataRow row = table.NewRow();
                row["Date"] = item.Date;
                row["Open"] = item.Open;
                row["High"] = item.High;
                row["Low"] = item.Low;
                row["Close"] = item.Close;
                row["Volume"] = item.Volume;
                row["AdjClose"] = item.AdjClose;
                row["StockId"] = item.StockId;
                table.Rows.Add(row);
            }
            return table;
        }
    }

    
}