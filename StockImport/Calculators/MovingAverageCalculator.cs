using System;
using StockImport.Domain;

namespace StockImport.Calculators
{
    /// <summary>
    /// Calculates a moving average value over a specified window.  The window size must be specified
    /// upon creation of this object.
    /// </summary>
    /// <remarks>Authored by Drew Noakes, February 2005.  Use freely, though keep this message intact and
    /// report any bugs to me.  I also appreciate seeing extensions, or simply hearing that you're using
    /// these classes.  You may not copyright this work, though may use it in commercial/copyrighted works.
    /// Happy coding.
    ///
    /// Updated 29 March 2007.  Added a Reset() method.</remarks>
    public class MovingAverageCalculator : ICalculator
    {
        private readonly int windowSize;
        private readonly decimal[] windowValues;
        private int oldestWindowValueIndex;
        private decimal sum;
        private int totalValuesInWindow;

        public MovingAverageCalculator(int windowSize)
        {
            if (windowSize < 1)
                throw new ArgumentOutOfRangeException("windowSize", windowSize, "Window size must be greater than zero.");

            this.windowSize = windowSize;
            this.windowValues = new decimal[this.windowSize];

            this.Reset();
        }

        /// <summary>
        /// Updates the moving average with its next value, and returns the updated average value.
        /// When IsMature is true and NextValue is called, a previous value will 'fall out' of the
        /// moving average.
        /// </summary>
        public decimal NextValue(decimal nextValue)
        {
            // add new value to the sum
            this.sum += nextValue;

            this.UpdateWindow(nextValue);

            this.IncrementOldestWindowValueIndex();

            return this.sum / this.totalValuesInWindow;
        }

        private void UpdateWindow(decimal nextValue)
        {
            if(!this.IsMature)
            {
                // we haven't yet filled our window
                this.totalValuesInWindow++;
            }
            else
            {
                // remove oldest value from sum
                this.sum -= this.windowValues[this.oldestWindowValueIndex];
            }

            // store the value
            this.windowValues[this.oldestWindowValueIndex] = nextValue;
        }

        private void IncrementOldestWindowValueIndex()
        {
            // progress the next value index pointer
            this.oldestWindowValueIndex++;
            if(this.oldestWindowValueIndex == this.windowSize)
            {
                this.oldestWindowValueIndex = 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether enough values have been provided to fill the
        /// speicified window size.  Values returned from NextValue may still be used prior
        /// to IsMature returning true, however such values are not subject to the intended
        /// smoothing effect of the moving average's window size.
        /// </summary>
        public bool IsMature
        {
            get { return this.totalValuesInWindow == this.windowSize; }
        }

        /// <summary>
        /// Clears any accumulated state and resets the calculator to its initial configuration.
        /// Calling this method is the equivalent of creating a new instance.
        /// </summary>
        public void Reset()
        {
            this.oldestWindowValueIndex = 0;
            this.sum = 0;
            this.totalValuesInWindow = 0;
        }
    }
}