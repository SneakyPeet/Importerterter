using System;
using System.Collections.Generic;
using FileHelpers;

namespace StockImport
{
    class DummyData
    {
        public static void Create()
        {
            var date = DateTime.Now;
            var stocklist = new List<Stock>();
            for (int i = 0; i < 10000; i++)
            {
                stocklist.Add(new Stock
                                  {
                                      Average = i,
                                      Close = i - 1,
                                      High = i,
                                      Low = -i,
                                      Open = i + (i / 2),
                                      Volume = i * 2.2m,
                                      //Date = date
                                  });
            }
            var engine = new FileHelperEngine<Stock>();
            engine.WriteFile(Constants.File, stocklist);
        }
    }
}