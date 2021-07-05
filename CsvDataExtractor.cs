using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CsvParser
{
    public class CsvDataExtractor
    {
        public List<T> ExtractData<T>(
            List<CsvTarget> targetList,
            string csvString,
            ExtractOptions options
        )
        where T : class
        {
            var genericType = typeof(T);

            PropertyInfo[] properties = genericType.GetProperties();
            List<string> csvLines = csvString.Split("\n").ToList();
            List<T> resultList = new();

            for (var index = 0; index < csvLines.Count(); index++)
            {
                string line = csvLines[index];
                bool isFirstIndexAndHaveHeader = options.HaveHeader && index.Equals(0);

                if (isFirstIndexAndHaveHeader)
                    continue;

                var result = GenerateObject<T>(
                    targetList,
                    properties,
                    line,
                    options
                );

                if (!options.ShouldRepeat)
                {
                    bool isAlreadyAdded = resultList.Contains(result);

                    if (!isAlreadyAdded)
                        resultList.Add(result);
                }
                else
                {
                    resultList.Add(result);
                }
            };

            return resultList;
        }

        private T GenerateObject<T>(
            List<CsvTarget> dataList,
            PropertyInfo[] properties,
            string line,
            ExtractOptions options
        )
        {
            List<string> columns = Regex.Split(line, $"{options.Separator}(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").ToList();
            var classType = typeof(T);

            T instance = (T)Activator.CreateInstance(classType);

            dataList.ForEach(data =>
            {
                int counter = 0;


                do
                {
                    var fieldsLastIndex = properties.Count() - 1;

                    PropertyInfo property = properties.Where(x => x.Name == data.FieldName).FirstOrDefault();

                    if (property is null)
                        throw new Exception($"Tipo ${classType} não contem a propriedade {data.FieldName}");

                    bool areNamesEqual = property.Name.Equals(data.FieldName);

                    if (areNamesEqual)
                    {
                        bool isOutOfBonds = data.CsvColumn >= columns.Count();

                        if (!isOutOfBonds)
                        {   
                            
                            property.SetValue(instance, columns[data.CsvColumn].Replace("\"", ""));
                            break;
                        }
                    }

                }
                while (counter++ < properties.Count());
            });

            return instance;
        }
    }
}