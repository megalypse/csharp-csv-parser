using CsvParser.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            CsvService.CheckOptions<T>(options);

            List<string> csvLines = csvString.Split("\n").ToList();
            List<T> resultList = new();

            for (var index = 0; index < csvLines.Count; index++)
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
            CsvService.CheckOptions<T>(options);

            List<string> csvLines = CsvService.BreakCsv(csvString);
            List<T> list = new();

            for (var i = 0; i < csvLines.Count; i++)
            {
                string line = csvLines[i];
                bool isFirstLoop = i.Equals(0);
                bool shouldSkipLoop = CsvService.ShouldSkipLoop(isFirstLoop, options.ShouldSkipHeader);

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

        private static T GenerateObject<T>(
            List<CsvTarget> targetList,
            string line,
            ExtractOptions options
        )
        {
            var classType = typeof(T);
            List<string> columns = CsvService.BreakLine(options.Separator, line);
            List<PropertyInfo> properties = CsvService.GetTypePropertyInfoList(classType);

            T instance = (T)Activator.CreateInstance(classType);

            targetList.ForEach(data =>
            {
                int counter = 0;

                do
                {
                    var fieldsLastIndex = properties.Count - 1;

                    PropertyInfo property = CsvService.GetPropertyOrThrow<T>(properties, data.FieldName);

                    bool isOutOfBonds = data.CsvColumn >= columns.Count;

                    if (!isOutOfBonds)
                    {
                        property.SetValue(instance, columns[data.CsvColumn].Replace("\"", ""));
                        break;
                    }

                }
                while (counter++ < properties.Count);
            });

            return instance;
        }

        private static T GenerateObject<T>(
            string line,
            ExtractOptions extractOptions
        )
        {
            Type typeOfGeneric = typeof(T);
            Type csvTargetAttributeType = typeof(CsvSourceColumnAttribute);


            T instance = (T)Activator.CreateInstance(typeOfGeneric);
            List<PropertyInfo> properties = instance.GetType().GetProperties().ToList();

            foreach (PropertyInfo property in properties)
            {
                List<string> columns = CsvService.BreakLine(extractOptions.Separator, line);

                CsvSourceColumnAttribute annotation = (CsvSourceColumnAttribute)property
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

        

        

        

        

        

        
    }
}
