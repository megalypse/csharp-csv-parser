using CsvParser;
using Example.Models;
using System.Diagnostics;
using System.Collections.Generic;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvString = $"name,age\njohn,18\njohanne,19";

            CsvDataExtractor extractor = new();
            List<CsvTarget> targetList = new()
            {
                new("Name", 0),
                new("Age", 1)
            };

            List<Person> personList = extractor.ExtractData<Person>(
                targetList,
                csvString,
                new ExtractOptions
                {
                    ShouldSkipHeader = true,
                    ShouldRepeatEquatableObject = false
            });

            Debug.Assert(personList.Count.Equals(2));
            Debug.Assert(personList[0].Name.Equals("john"));
            Debug.Assert(personList[0].Age.Equals("18"));
            Debug.Assert(personList[1].Name.Equals("johanne"));
            Debug.Assert(personList[1].Age.Equals("19"));

            List<TargetedPerson> targetedPersonList = extractor.ExtractData<TargetedPerson>(
                csvString, 
                new ExtractOptions
                { 
                    ShouldSkipHeader = true,
                    ShouldRepeatEquatableObject = false 
            });

            Debug.Assert(targetedPersonList.Count.Equals(2));
            Debug.Assert(targetedPersonList[0].Name.Equals("john"));
            Debug.Assert(targetedPersonList[0].Age.Equals("18"));
            Debug.Assert(targetedPersonList[1].Name.Equals("johanne"));
            Debug.Assert(targetedPersonList[1].Age.Equals("19"));
        }
    }
}
