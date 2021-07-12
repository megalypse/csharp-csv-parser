using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class Person : IEquatable<Person>
    {
        public string Name { get; set; }

        public bool Equals(Person other) =>
            Name.Equals(other.Name);
    }
}
