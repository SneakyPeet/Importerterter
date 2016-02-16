using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileHelpers;

namespace StockImport
{
    public static class FileExtensions
    {
        public static int ProcessAndConcatToFile(this string directory, string outputFile)
        {
            return directory
                .Process()
                .Write(outputFile);
        }

        public static List<Stock> Process(this string directory)
        {
            var files = Directory.GetFiles(directory);
            var all = new List<Stock>();
            var fileEngine = new FileHelperEngine<StockImport>();
            foreach (var file in files)
            {
                all.AddRange(file.ReadFile(fileEngine)
                    .Order());
            }

            return all;
        }

        public static IEnumerable<Stock> ProcessFile(this string file)
        {
            var fileEngine = new FileHelperEngine<StockImport>();
            return file
                .ReadFile(new FileHelperEngine<StockImport>())
                .Order();
        }

        public static IEnumerable<Stock> ReadFile(this string file, FileHelperEngine<StockImport> fileEngine)
        {
            var stockId = Path.GetFileNameWithoutExtension(file);
            var data = fileEngine.ReadFile(file);
            return data.Select(x => new Stock(x, stockId));
        }

        public static IEnumerable<Stock> Order(this IEnumerable<Stock> stock)
        {
            return stock.OrderBy(x => x.Date);
        }

        public static IEnumerable<Stock> Clean(this IEnumerable<Stock> stock)
        {
            throw new NotImplementedException();
        }

        public static int Write(this List<Stock> stock, string path)
        {
            var fileEngine = new FileHelperEngine<Stock>();
            fileEngine.HeaderText = fileEngine.GetFileHeader();
            fileEngine.WriteFile(path, stock);
            return stock.Count();
        }
    }
}