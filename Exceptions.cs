using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    public class ILSException : Exception
    {
        public ILSException(string m) : base("Exception thrown on line " + LinePointer.GetCurrentLine() +": " + m) { }
    }
}