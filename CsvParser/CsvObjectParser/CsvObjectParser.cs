using CsvParser.Annotations;
using CsvParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CsvParser.CsvObjectParser
{
    public class CsvObjectParser
    {
        public List<T> Parse<T>(
            List<CsvTarget> targetList,
            string csvString,
            ExtractOptions options
        )
        where T : class
        {
            CheckOptions<T>(options);

            List<string> csvLines = csvString.Split("\n").ToList();
            List<T> resultList = new();

            for (var index = 0; index < csvLines.Count(); index++)
            {
                string line = csvLines[index];
                bool isFirstIndexAndHaveHeader = options.ShouldSkipHeader && index.Equals(0);

                if (isFirstIndexAndHaveHeader)
                    continue;

                var result = GenerateObject<T>(
                    targetList,
                    line,
                    options
                );

                if (options.ShouldSkipEqualObject is true)
                {
                    bool isAlreadyPresent = resultList.Contains(result);
                    if (isAlreadyPresent is false)  resultList.Add(result);
                }
                else 
                    resultList.Add(result);
            };

            return resultList;
        }

        public List<T> Parse<T>(
            string csvString,
            ExtractOptions options
        )
        {
            CheckOptions<T>(options);
            List<string> csvLines = BreakCsv(csvString);
            List<T> list = new();

            for (var i = 0; i < csvLines.Count; i++)
            {
                string line = csvLines[i];
                bool isFirstLoop = i.Equals(0);
                bool shouldSkipLoop = ShouldSkipLoop(isFirstLoop, options.ShouldSkipHeader);

                if (shouldSkipLoop) continue;

                T instance = GenerateObject<T>(line, options);

                if (options.ShouldSkipEqualObject is true)
                {
                    bool isAlreadyPresent = list.Contains(instance);
                    if (isAlreadyPresent is false) list.Add(instance);
                }
                else
                    list.Add(instance);
            }

            return list;
        }

        private T GenerateObject<T>(
            List<CsvTarget> targetList,
            string line,
            ExtractOptions options
        )
        {
            var classType = typeof(T);
            List<string> columns = BreakLine(options.Separator, line);
            List<PropertyInfo> properties = GetTypePropertyInfoList(classType);

            T instance = (T)Activator.CreateInstance(classType);

            targetList.ForEach(data =>
            {
                int counter = 0;

                do
                {
                    var fieldsLastIndex = properties.Count() - 1;

                    PropertyInfo property = GetPropertyOrThrow<T>(properties, data.FieldName);

                    bool isOutOfBonds = data.CsvColumn >= columns.Count();

                    if (!isOutOfBonds)
                    {
                        property.SetValue(instance, columns[data.CsvColumn].Replace("\"", ""));
                        break;
                    }

                }
                while (counter++ < properties.Count());
            });

            return instance;
        }

        private T GenerateObject<T>(
            string line,
            ExtractOptions extractOptions
        )
        {
            Type typeOfGeneric = typeof(T);
            Type csvTargetAttributeType = typeof(SourceColumnAttribute);


            T instance = (T)Activator.CreateInstance(typeOfGeneric);
            List<PropertyInfo> properties = instance.GetType().GetProperties().ToList();

            foreach (PropertyInfo property in properties)
            {
                List<string> columns = BreakLine(extractOptions.Separator, line);

                SourceColumnAttribute annotation = (SourceColumnAttribute)property
                    .GetCustomAttributes(csvTargetAttributeType, false)
                    .First();

                if (!(annotation.Equals(null)))
                {
                    int columnNumber = annotation.columnNumber;
                    string columnValue = columns[columnNumber];

                    property.SetValue(instance, columnValue);
                }
            }

            return instance;
        }

        private void CheckOptions<T>(ExtractOptions options)
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

        private string ColumnSeparatorRgx(string separator) => $"{separator}(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";

        private List<string> BreakLine(string separator, string line) =>
            Regex.Split(line, ColumnSeparatorRgx(separator)).ToList();

        private List<string> BreakCsv(string csvString) =>
            csvString.Split("\n").ToList();

        private bool ShouldSkipLoop(bool isFirstLoop, bool haveHeader) =>
            isFirstLoop && haveHeader;

        private PropertyInfo GetPropertyOrThrow<T>(List<PropertyInfo> propertyList, string desiredName)
        {
            PropertyInfo property = propertyList
                        .Where(x => x.Name.Equals(desiredName))
                        .FirstOrDefault();

            if (property is null)
                throw new Exception($"Type ${typeof(T)} doesn't contain {desiredName} property.");

            return property;
        }

        private List<PropertyInfo> GetTypePropertyInfoList(Type type) => type.GetProperties().ToList();
    }
}
