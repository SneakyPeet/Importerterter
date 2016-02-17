using StockImport.Domain;

namespace StockImport.Calculators
{
    public class MacdCalculator : ICalculator
    {
        private readonly EmaCalculator leadingEma;
        private readonly EmaCalculator laggingEma;

        public MacdCalculator(int leadingWindowSize, int lagginWindowSize)
        {
            this.leadingEma = new EmaCalculator(leadingWindowSize);
            this.laggingEma = new EmaCalculator(lagginWindowSize);
            Reset();
        }
        
        public decimal NextValue(decimal nextValue)
        {
            return leadingEma.NextValue(nextValue) - laggingEma.NextValue(nextValue);
        }
        
        public void Reset()
        {
            leadingEma.Reset();
        }
    }
}