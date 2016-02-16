using System;
using StockImport.Domain;

namespace StockImport.Storage
{
    public interface IShareRepository : IDisposable
    {
        void Save(Share share);
    }
}