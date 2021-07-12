using CsvParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsvParser
{
    static class CsvService
    {
        public static void CheckOptions<T>(ExtractOptions options)
        {
            if (options.ShouldSkipEqualObject is true)
            {
                List<Type> typeInterfaces = typeof(T).GetInterfaces().ToList();
                bool containsEquatable = typeInterfaces.Contains(typeof(IEquatable<T>));

                if (containsEquatable is false)
                {
                    string errorMessage = "Class must implement System.IEquatable interface if ShouldRepeat is False.";

                    throw new InterfaceNotFoundException(errorMessage);
                }
            }
        }

        public static List<string> BreakLine(string separator, string line) =>
            Regex.Split(line, ColumnSeparatorRgx(separator)).ToList();

        public static List<string> BreakCsv(string csvString) =>
            csvString.Split("\n").ToList();

        public static bool ShouldSkipLoop(bool isFirstLoop, bool haveHeader) =>
            isFirstLoop && haveHeader;

        public static PropertyInfo GetPropertyOrThrow<T>(List<PropertyInfo> propertyList, string desiredName)
        {
            PropertyInfo property = propertyList
                        .Where(x => x.Name.Equals(desiredName))
                        .FirstOrDefault();

            if (property is null)
                throw new Exception($"Type ${typeof(T)} doesn't contain {desiredName} property.");

            return property;
        }

        public static List<PropertyInfo> GetTypePropertyInfoList(Type type) => type.GetProperties().ToList();

        static string ColumnSeparatorRgx(string separator) => 
            $"{separator}(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
    }
}
