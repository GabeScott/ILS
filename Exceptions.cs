using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    abstract class ILSException : Exception
    {

        public ILSException(string m) : base(m) { }

    }

    class TooManyTokensException : ILSException
    {
        public TooManyTokensException(string m) : base("Error of type ILSTooManyExceptions thrown: " + m) { }
    }

    class TooFewTokensException : ILSException
    {
        public TooFewTokensException(string m) : base("Error of type ILSTooFewExceptions thrown: " + m) { }
    }

    class InvalidNumberValueException : ILSException
    {
        public InvalidNumberValueException(string m) : base("Error of type InvalidNumberValueException thrown: " + m) { }
    }

    class InvalidBeginFileException : ILSException
    {
        public InvalidBeginFileException(string m) : base("Error of type InvalidBeginFileException thrown: " + m) { }
    }

    class InvalidEndFileException : ILSException
    {
        public InvalidEndFileException(string m) : base("Error of type InvalidEndFileException thrown: " + m) { }
    }

    class InvalidEndLineException : ILSException
    {
        public InvalidEndLineException(string m) : base("Error of type InvalidEndLineException thrown: " + m) { }
    }

    class InvalidTokenException : ILSException
    {
        public InvalidTokenException(string m) : base("Error of type InvalidTokenException thrown: " + m) { }
    }

    class NoTokenInLineException : ILSException
    {
        public NoTokenInLineException(string m) : base("Error of type NoTokenInLineException thrown: " + m) { }
    }




}
