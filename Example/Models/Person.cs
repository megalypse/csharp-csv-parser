using System;

namespace Example.Models
{
    public class Person : IEquatable<Person>
    {
        public string Name { get; set; }

        public string Age {  get; set; }

        public bool Equals(Person other)
        {
            return Name.Equals(other.Name) && Age.Equals(other.Age);
        }

        public override string ToString()
        {
            return $"Name({Name})\nAge({Age})\n";
        }
    }
}
