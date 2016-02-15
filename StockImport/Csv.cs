using System.Collections.Generic;
using System.IO;
using FileHelpers;

namespace StockImport
{
    class Csv : IDb
    {
        public int Import(IEnumerable<string> files)
        {
            var all = new List<Stock>();
            var fileEngine = new FileHelperEngine<Stock>();
            foreach (var file in files)
            {
                var stockId = Path.GetFileNameWithoutExtension(file);
                var data = fileEngine.ReadFile(file);
                foreach (var stock in data)
                {
                    stock.StockId = stockId;
                }
                all.AddRange(data);
            }
            fileEngine.WriteFile("ALLSTOCKS.CSV", all);
            return all.Count;
        }
    }
}