using CsvParser.Annotations;
using System;

namespace Example.Models
{
    public class TargetedPerson : IEquatable<TargetedPerson>
    {
        [ColumnTarget(0)]
        public string Name { get; set; }

        [ColumnTarget(1)]
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
