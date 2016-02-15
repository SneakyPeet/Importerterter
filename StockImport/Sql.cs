using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using FileHelpers;

namespace StockImport
{
    class Sql : IDb
    {
        private const string connectionString = "Server=.;Database=Stocks;Trusted_Connection=True;";
        private const string tableName = "dbo.Stocks";
        private const int batchSize = 100;

        public int Import(IEnumerable<string> files)
        {
            var fileReader = new FileHelperEngine<Stock>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var file in files)
                {
                    var stockId = Path.GetFileNameWithoutExtension(file);
                    var data = fileReader.ReadFile(file).AsDataTable(stockId);
                    
                    var transaction = connection.BeginTransaction();

                    using (var bulkCopy = new SqlBulkCopy(connection,SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = batchSize;
                        bulkCopy.DestinationTableName = tableName;
                        try
                        {
                            bulkCopy.WriteToServer(data);
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            connection.Close();
                            throw;
                        }
                    }
                    transaction.Commit();
                    
                }
            }
            return 0;
        }
    }

    public static class IEnumerableExtensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> data, string stockId)
        {
            const string si = "StockId";
            var type = typeof(T);
            var fields = type.GetFields();
            var table = new DataTable();
            table.Columns.Add(si, typeof(string));
            foreach (var field in fields)
                table.Columns.Add(field.Name, field.FieldType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                row[si] = stockId;
                foreach (var field in fields)
                    row[field.Name] = field.GetValue(item);
                table.Rows.Add(row);
            }
            return table;
        }
    }
}