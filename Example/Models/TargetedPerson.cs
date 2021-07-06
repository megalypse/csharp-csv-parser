using CsvParser.Annotations;
using System;

namespace Example.Models
{
    public class TargetedPerson : IEquatable<TargetedPerson>
    {
        [SourceColumn(0)]
        public string Name { get; set; }

        [SourceColumn(1)]
        public string Age { get; set; }

        public bool Equals(TargetedPerson other)
        {
            return Name.Equals(other.Name) && Age.Equals(other.Age);
        }

        public override string ToString()
        {
            return $"Name({Name})\nAge({Age})\n";
        }
    }
}
