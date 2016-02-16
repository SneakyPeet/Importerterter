using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StockImport
{
    class Program
    {
        
        private const string filesDirectory = "Data";
        private const string outputFile = "ALLSTOCKS.CSV";
        private const string db = "db.accdb";
        
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var total = Directory
                .GetFiles(filesDirectory)
                .ImportToSql(FileExtensions.ProcessFile);

            stopWatch.Stop();

            Console.WriteLine(total + " rows imported in " + stopWatch.Elapsed);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        
    }
}
