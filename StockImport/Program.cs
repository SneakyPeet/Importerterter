using System;
using System.Diagnostics;
using StockImport.Calculators;
using StockImport.Domain;
using StockImport.Storage;

namespace StockImport
{
    class Program
    {
        
        private const string filesDirectory = "C:\\Development\\stocks\\Data";
        private const string connectionString = "Server=.;Database=Stocks;Trusted_Connection=True;";
        private const string tableName = "dbo.Stocks";
        
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var total = Import();

            stopWatch.Stop();

            Console.WriteLine(total + " rows imported in " + stopWatch.Elapsed);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static int Import()
        {
            var processEngine = BootstrapProcessEngine();
            using (var repo = new ShareRepository(connectionString, tableName))
            {
                var importer = new ShareImporter(repo, processEngine);
                return importer.Import(filesDirectory);
            }
        }

        private static ProcessEngine BootstrapProcessEngine()
        {
            var engine = new ProcessEngine();
            engine.Add(Calculation.Sma12, new MovingAverageCalculator(12));
            engine.Add(Calculation.Sma20, new MovingAverageCalculator(20));
            engine.Add(Calculation.Sma50, new MovingAverageCalculator(50));
            return engine;
        }
    }
}
