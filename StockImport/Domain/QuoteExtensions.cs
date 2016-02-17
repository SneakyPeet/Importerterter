using System.Collections.Generic;
using System.Linq;

namespace StockImport.Domain
{
    public static class QuoteExtensions
    {
        public static IEnumerable<IQuote> Order(this IEnumerable<IQuote> stock)
        {
            return stock.OrderBy(x => x.Date);
        }

        public static IEnumerable<IQuote> Clean(this IEnumerable<IQuote> stock)
        {
            //todo
            return stock;
        }

        public static List<ProcessedQuote> ToProcessableStock(this IEnumerable<IQuote> stock)
        {
            return stock
                .Select(x => new ProcessedQuote(x))
                .ToList();
        }
    }
}