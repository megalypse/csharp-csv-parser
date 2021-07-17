using CsvParser;
using CsvParser.CsvObjectParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Models;
using Xunit;

namespace Tests
{
    public class CsvObjectParserTest
    {
        private CsvObjectParser _parser;
        private readonly string CsvString = "name\njohn\njohanne";

        public CsvObjectParserTest()
        {
            _parser = new CsvObjectParser();
        }

        [Fact]
        public void ShouldReturnObjectList()
        {
            List<CsvTarget> targetList = new()
            {
                new("Name", 0),
            };

            List<Person> personList = _parser.Parse<Person>(
                targetList,
                CsvString,
                new() { 
                    ShouldSkipHeader = true
                });

            Assert.Equal(2, personList.Count);
            Assert.Equal("john", personList[0].Name);
            Assert.Equal("johanne", personList[1].Name);
        }

        [Fact]
        public void ShouldReturnObjectList_CustomAttributeMethod()
        {
            List<Person> personList = _parser.Parse<Person>(
                CsvString,
                new()
                {
                    ShouldSkipHeader = true
                });

            Assert.Equal(2, personList.Count);
            Assert.Equal("john", personList[0].Name);
            Assert.Equal("johanne", personList[1].Name);
        }
    }
}
