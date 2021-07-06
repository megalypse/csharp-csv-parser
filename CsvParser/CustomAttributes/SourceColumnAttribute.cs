using System;

namespace CsvParser.Annotations
{
    public class SourceColumnAttribute : Attribute
    {
        
        public int columnNumber;

        public SourceColumnAttribute(int columnNumber)
        {
            this.columnNumber = columnNumber;
        }
    }
}
