using System;
using System.Diagnostics;
using System.IO;

namespace StockImport
{
    class Program
    {
        
        private const string filesDirectory = "Data";
        
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var files = Directory.GetFiles(filesDirectory);

            IDb csv = new Csv();
            var total = csv.Import(files);
            stopWatch.Stop();

            Console.WriteLine(total + " rows imported in " + stopWatch.Elapsed);

            stopWatch.Restart();
            var import = new AccessCsv();
            import.Import();
            stopWatch.Stop();

            Console.WriteLine("Access done in " + stopWatch.Elapsed);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        
    }
}
