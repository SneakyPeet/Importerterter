using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StockImport.Calculators;
using StockImport.Domain;
using StockImport.Import;
using StockImport.Storage;

namespace StockImport
{
    class Program
    {
        
        private const string filesDirectory = "C:\\Development\\stocks\\Data2";
        private const string connectionString = "Server=.;Database=Stocks;Trusted_Connection=True;";
        private const string tableName = "dbo.Stocks";
        private const int concurrentBatches = 10;
        
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
            var batches = GetBatches(filesDirectory, concurrentBatches);
            var total = 0;
            Parallel.For(0, batches.Count(),
                index => {
                    var t = ProcessBatch(batches.ElementAt(index));
                    Interlocked.Add(ref total, t);
                   });
            return total;
        }

        private static ProcessEngine BootstrapProcessEngine()
        {
            var engine = new ProcessEngine();
            engine.Add(Calculation.Sma12, new SmaCalculator(12));
            engine.Add(Calculation.Sma20, new SmaCalculator(20));
            engine.Add(Calculation.Sma50, new SmaCalculator(50));
            engine.Add(Calculation.Ema12, new EmaCalculator(12));
            engine.Add(Calculation.Ema20, new EmaCalculator(20));
            return engine;
        }

        private static List<string[]> GetBatches(string directory, int batchTotal)
        {
            var files = directory.Files();
            var fileTotal = files.Length;
            var batchSize = (int)Math.Ceiling(fileTotal / (decimal)batchTotal);
            var index = 0;
            var batches = new List<string[]>();
            while (index < fileTotal)
            {
                var batch = files.Skip(index).Take(batchSize).ToArray();
                batches.Add(batch);
                index += batchSize;
            }
            return batches;
        }

        private static int ProcessBatch(IEnumerable<string> files)
        {
            var totalRecords = 0;
            var processEngine = BootstrapProcessEngine();
            using (var repo = new ShareRepository(connectionString, tableName))
            {
                foreach (var file in files)
                {
                    processEngine.Reset();
                    var shareId = file.ToShareId();
                    var quotes = file.ReadFile();
                    var share = new Share(shareId, quotes, processEngine);
                    repo.Save(share);
                    totalRecords += share.ProcessedQuotes.Count;
                }
            }

            return totalRecords;
        }

        
    }
}
