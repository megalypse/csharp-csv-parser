using CsvParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tests.Models;
using Xunit;

namespace Tests
{
    public class CsvServiceTest
    {
        [Fact]
        public void ShouldNotThrowIfEquatableImpl()
        {
            ExtractOptions options = new ExtractOptions()
            {
                ShouldSkipEqualObject = true
            };

            CsvService.CheckOptions<Person>(options);
        }

        [Fact]
        public void ShouldThrow_IfNotEquatableImpl()
        {
            ExtractOptions options = new ExtractOptions()
            {
                ShouldSkipEqualObject = true
            };

            Assert.ThrowsAny<Exception>(() => CsvService.CheckOptions<Bug>(options));
        }

        [Fact]
        public void Returns_TwoStrings_OnSuccess()
        {
            string twoStatements = "statement1;statement2";
            
            var splittedLine = CsvService.BreakLine(";", twoStatements);

            Assert.Equal(2, splittedLine.Count);
            Assert.Equal("statement1", splittedLine[0]);
            Assert.Equal("statement2", splittedLine[1]);
        }

        [Fact]
        public void ReturnOneStringDueLackOfSeparator()
        {
            string unseparatedString = "statement1,statement2";

            var splittedStr = CsvService.BreakLine(";", unseparatedString);

            Assert.Single(splittedStr);
            Assert.Equal(unseparatedString, splittedStr[0]);
        }

        [Fact]
        public void ShouldBreakCsvIntoLines()
        {
            string csvString = "line1\nline2";

            var csvLines = CsvService.BreakCsv(csvString);

            Assert.Equal(2, csvLines.Count);
            Assert.Equal("line1", csvLines[0]);
            Assert.Equal("line2", csvLines[1]);
        }

        [Fact]
        public void ShouldReturnTrue()
        {
            bool result = CsvService.ShouldSkipLoop(true, true);

            Assert.True(result);
        }

        [Fact]
        public void ShouldReturnClassPropertyInfoList()
        {
            List<PropertyInfo> result = CsvService.GetTypePropertyInfoList(typeof(Person));

            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldReturnDesiredProperty()
        {
            List<PropertyInfo> propertyList = CsvService.GetTypePropertyInfoList(typeof(Person));

            PropertyInfo result = CsvService.GetPropertyOrThrow<Person>(propertyList, "Name");

            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldThrowDueToUnexistentProperty()
        {
            List<PropertyInfo> propertyList = CsvService.GetTypePropertyInfoList(typeof(Person));

            Assert.Throws<Exception>(() => 
                CsvService.GetPropertyOrThrow<Person>(propertyList, "Voltage"));
        }
    }
}
