using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileHelpers;
using StockImport.Domain;

namespace StockImport.Import
{
    public static class ImportExtensions
    {
        public static string ToShareId(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static string[] Files(this string directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }

        public static IEnumerable<IQuote> ReadFile(this string filePath)
        {
            var fileEngine = new DelimitedFileEngine<QuoteImport>();
            return fileEngine.ReadFile(filePath);
        }

        public static int Write(this List<ProcessedQuote> stock, string path)
        {
            var fileEngine = new FileHelperEngine<ProcessedQuote>();
            fileEngine.HeaderText = fileEngine.GetFileHeader();
            fileEngine.WriteFile(path, stock);
            return stock.Count();
        }
    }
}