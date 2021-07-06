using System;

namespace CsvParser.Annotations
{
    public class ColumnTargetAttribute : Attribute
    {
        
        public int columnNumber;

        public ColumnTargetAttribute(int columnNumber)
        {
            this.columnNumber = columnNumber;
        }
    }
}
