using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileHelpers;
using StockImport.Domain;

namespace StockImport.Helpers
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

        public static IEnumerable<Quote> ReadFile(this string filePath)
        {
            var fileEngine = new DelimitedFileEngine<Quote>();
            return fileEngine.ReadFile(filePath);
        }

        public static IEnumerable<Quote> Order(this IEnumerable<Quote> stock)
        {
            return stock.OrderBy(x => x.Date);
        }

        public static IEnumerable<Quote> Clean(this IEnumerable<Quote> stock)
        {
            //todo
            return stock;
        }

        public static List<ProcessedQuote> ToProcessableStock(this IEnumerable<Quote> stock)
        {
            return stock
                .Select(x => new ProcessedQuote(x))
                .ToList();
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