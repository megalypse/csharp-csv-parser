using System;

namespace CsvParser.Annotations
{
    public class CsvTargetMarkAttribute : Attribute
    {
        
        public int columnNumber;

        public CsvTargetMarkAttribute(int columnNumber)
        {
            this.columnNumber = columnNumber;
        }
    }
}
