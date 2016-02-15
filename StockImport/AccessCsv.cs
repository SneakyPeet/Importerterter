using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace StockImport
{
    class AccessCsv
    {
        public void Import()
        {
            var db = "db.accdb";
            var path = System.Environment.CurrentDirectory;
            var query = string.Format("INSERT INTO Stocks SELECT * FROM [Text;FMT=Delimited;HDR=YES;Database=.].[ALLSTOCKS.CSV]", path, db);
            using (var connection = new OleDbConnection((@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb;")))
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