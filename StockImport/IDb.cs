using System.Collections.Generic;

namespace StockImport
{
    interface IDb
    {
        int Import(IEnumerable<string> files);
    }
}