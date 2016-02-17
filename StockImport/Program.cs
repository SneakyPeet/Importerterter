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
        private const int concurrentBatches = 8;
        
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

        private static int ProcessBatch(string[] files)
        {
            using (var repo = new ShareRepository(connectionString, tableName))
            {
                var engine = BootstrapProcessEngine();
                var importer = new ShareImporter(repo, engine);
                return importer.Import(files);
            }

        }

        private static ProcessEngine BootstrapProcessEngine()
        {
            var engine = new ProcessEngine();
            engine.Add(Calculation.Ema12, new EmaCalculator(12));
            engine.Add(Calculation.Ema13, new EmaCalculator(13));
            engine.Add(Calculation.Ema14, new EmaCalculator(14));
            engine.Add(Calculation.Ema15, new EmaCalculator(15));
            engine.Add(Calculation.Ema26, new EmaCalculator(26));
            engine.Add(Calculation.Ema39, new EmaCalculator(39));
            engine.Add(Calculation.Ema42, new EmaCalculator(42));
            engine.Add(Calculation.Ema45, new EmaCalculator(45));
            engine.Add(Calculation.Ema55, new EmaCalculator(55));
            engine.Add(Calculation.Ema100, new EmaCalculator(100));
            engine.Add(Calculation.MACD12_26, new MacdCalculator(12, 26));	
            engine.Add(Calculation.MACDX12_26, new MacdCalculator(12, 26));		
            engine.Add(Calculation.MACD13_39, new MacdCalculator(13, 39));		
            engine.Add(Calculation.MACDX13_39, new MacdCalculator(13, 39));	
            engine.Add(Calculation.MACD14_42, new MacdCalculator(14, 42));	
            engine.Add(Calculation.MACDX14_42, new MacdCalculator(14, 42));	
            engine.Add(Calculation.MACD15_45, new MacdCalculator(15, 45));	
            engine.Add(Calculation.MACDX15_45, new MacdCalculator(15, 45));	
            engine.Add(Calculation.MACD15_55, new MacdCalculator(15, 55));
            engine.Add(Calculation.MACDX15_55, new MacdCalculator(15, 55));	
            return engine;
        }
    }
}
