using CsvParser.Annotations;
using System;

namespace Tests.Models
{
    public class Person : IEquatable<Person>
    {
        [CsvSourceColumn(0)]
        public string Name { get; set; }

        public bool Equals(Person other) =>
            Name.Equals(other.Name);
    }
}
