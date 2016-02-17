using StockImport.Import;
using StockImport.Storage;

namespace StockImport.Domain
{
    public class ShareImporter
    {
        private readonly IShareRepository repo;
        private readonly ProcessEngine engine;

        public ShareImporter(IShareRepository repo, ProcessEngine engine)
        {
            this.repo = repo;
            this.engine = engine;
        }

        //todo explore parralelization here
        public int Import(string[] files)
        {
            var totalRecords = 0;
            engine.Reset();
            foreach(var file in files)
            {
                var share = new Share(
                    file.ToShareId(),
                    file.ReadFile(),
                    engine
                );
                repo.Save(share);
                totalRecords += share.ProcessedQuotes.Count;
            }
            return totalRecords;
        }
    }
}