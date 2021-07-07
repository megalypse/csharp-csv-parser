using System;

namespace CsvParser.Annotations
{
    public class CsvSourceColumnAttribute : Attribute
    {
        
        public int columnNumber;

        public CsvSourceColumnAttribute(int columnNumber)
        {
            this.columnNumber = columnNumber;
        }
    }
}
