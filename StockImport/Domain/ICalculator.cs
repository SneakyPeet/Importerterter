namespace StockImport.Domain
{
    public interface ICalculator
    {
        decimal NextValue(decimal value);

        void Reset();
    }
}