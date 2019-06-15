using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    abstract class Exceptions : Exception
    {

        public Exceptions(string m) : base(m) { }

    }

    class ILSTooManyTokensException : Exceptions
    {
        public ILSTooManyTokensException(string m) : base("Error of type ILSTooManyExceptions thrown: " + m) { }
    }

    class ILSTooFewTokensException : Exceptions
    {
        public ILSTooFewTokensException(string m) : base("Error of type ILSTooFewExceptions thrown: " + m) { }
    }


}
