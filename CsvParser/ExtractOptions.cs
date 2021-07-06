namespace CsvParser
{
    public class ExtractOptions
    {
        public string Separator { get; set; } = ",";

        public bool ShouldSkipHeader { get; set; } = false;

        public bool ShouldSkipEqualObject { get; set; } = true;
    }
}