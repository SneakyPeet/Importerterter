using System.Data.OleDb;

namespace StockImport
{
    static class AccessExtensions
    {
        public static void ImportInto(this string file, string db)
        {
            var query = string.Format("INSERT INTO Stocks SELECT * FROM [Text;FMT=Fixed;HDR=YES;Database=.;].[{0}]", file);
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};",db);

            using (var connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var cmd = new OleDbCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }
    }
}