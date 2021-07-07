using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvParser.Exceptions
{
    class InterfaceNotFoundException : Exception
    {
        public InterfaceNotFoundException(string message) : base(message)
        {
        }
    }
}
