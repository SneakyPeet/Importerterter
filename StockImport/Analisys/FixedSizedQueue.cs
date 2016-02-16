using System.Collections.Generic;

namespace StockImport.Analisys
{
    public class FixedSizedQueue : List<decimal>
    {
        public FixedSizedQueue(int size, decimal start)
        {
            this.Size = size;
            for(int i = 0; i < size; i++)
            {
                this.Enqueue(start);
            }
        }


        public int Size { get; private set; }
        public void Enqueue(decimal value)
        {
            this.Add(value);
            this.RemoveAt(0);
        }
    }
}