namespace StockImport.Analisys
{
    public class Nothing
    {
        public static Nothing Value()
        {
            return new Nothing();
        }

        public override string ToString()
        {
            return "Nothing";
        }
    }
}