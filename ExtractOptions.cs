namespace CsvParser
{
    public class ExtractOptions
    {
        public string Separator { get; set; } = ",";

        public bool HaveHeader { get; set; } = false;

        public bool ShouldRepeat { get; set; } = true;
    }
}