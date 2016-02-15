using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FileHelpers;

namespace StockImport
{
    //http://stackoverflow.com/a/7080644/1338352
    class Access : IDb
    {
        private const string dbFile = "db.accdb";
        private const string tableName = "Stocks";
        private const string deleteQuery = "DELETE FROM " + tableName;

        public int Import(IEnumerable<string> files)
        {
            //Setup database engine and record Fields
            var fileReader = new FileHelperEngine<Stock>();
            var dbEngine = new Microsoft.Office.Interop.Access.Dao.DBEngine();
            var db = dbEngine.OpenDatabase(dbFile);
            //db.Execute(deleteQuery);
            var recordSet = db.OpenRecordset(tableName);
            
            //insert
            dbEngine.BeginTrans();
            var insertCounter = 0;
            foreach (var file in files)
            {
                var rows = fileReader.ReadFile(file);
                var stockId = Path.GetFileNameWithoutExtension(file);
                foreach (var row in rows)
                {
                    recordSet.AddNew();
                    //should match columns in table
                    recordSet.Fields[1].Value = row.Average;
                    recordSet.Fields[2].Value = row.Close;
                    recordSet.Fields[3].Value = row.High;
                    recordSet.Fields[4].Value = row.Low;
                    recordSet.Fields[5].Value = row.Open;
                    recordSet.Fields[6].Value = row.Volume;
                    recordSet.Fields[7].Value = row.Date;
                    recordSet.Fields[8].Value = stockId;
                    recordSet.Update();
                    if (0 == insertCounter % 5000)
                    {
                        dbEngine.CommitTrans();
                        dbEngine.BeginTrans();
                    }
                    insertCounter++;
                }
            }
            dbEngine.CommitTrans();
            recordSet.Close();
            db.Close();
            return insertCounter;
        }
    }
}