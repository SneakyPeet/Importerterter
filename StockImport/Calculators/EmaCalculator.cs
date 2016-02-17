using System;
using StockImport.Domain;

namespace StockImport.Calculators
{
    public class EmaCalculator : ICalculator
    {
        private readonly SmaCalculator sma;
        private readonly decimal multiplier;
        private decimal previousEma;
        private bool isMature;

        public EmaCalculator(int windowSize)
        {
            if (windowSize < 1)
                throw new ArgumentOutOfRangeException("windowSize", windowSize, "Window size must be greater than zero.");

            this.sma = new SmaCalculator(windowSize);
            this.multiplier = 2m / (windowSize + 1);
            Reset();
        }
        
        public decimal NextValue(decimal nextValue)
        {
            if (isMature)
            {
                previousEma = (nextValue - previousEma) * multiplier + previousEma;
            }
            else
            {
                var smaValue = sma.NextValue(nextValue);
                previousEma = smaValue;
                isMature = sma.IsMature;
            }
            
            return previousEma;
        }
        
        public void Reset()
        {
            sma.Reset();
            previousEma = 0;
            isMature = false;
        }
    }
}