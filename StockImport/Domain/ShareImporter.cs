using StockImport.Helpers;
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

        public int Import(string directory)
        {
            var files = directory.Files();
            var totalRecords = 0;
            foreach(var file in files)
            {
                processEngine.Reset();
                var share = new Share(file, processEngine);
                repo.Save(share);
                totalRecords += share.ProcessedQuotes.Count;
            }
            return totalRecords;
        }
    }
}