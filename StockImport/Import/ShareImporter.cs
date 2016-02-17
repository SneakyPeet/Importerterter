using StockImport.Import;
using StockImport.Storage;

namespace StockImport.Domain
{
    public class ShareImporter
    {
        private readonly IShareRepository repo;
        private readonly ProcessEngine processEngine;

        public ShareImporter(IShareRepository repo, ProcessEngine processEngine)
        {
            this.repo = repo;
            this.processEngine = processEngine;
        }

        public int Import(string[] files)
        {
            var totalRecords = 0;
            foreach(var file in files)
            {
                processEngine.Reset();
                var shareId = file.ToShareId();
                var quotes = file.ReadFile();
                var share = new Share(shareId, quotes, processEngine);
                repo.Save(share);
                totalRecords += share.ProcessedQuotes.Count;
            }
            return totalRecords;
        }
    }
}