namespace CsvParser
{
    public struct CsvTarget
    {
        public CsvTarget(string fieldName, int csvColumn)
        {
            FieldName = fieldName;
            CsvColumn = csvColumn;
        }
        
        public string FieldName;

        public int CsvColumn;

    }
}