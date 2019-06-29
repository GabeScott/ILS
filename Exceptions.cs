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
        public TooManyTokensException(string m) : base("ILSTooManyTokensExceptions thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class TooFewTokensException : ILSException
    {
        public TooFewTokensException(string m) : base("ILSTooFewTokensException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidNumberValueException : ILSException
    {
        public InvalidNumberValueException(string m) : base("InvalidNumberValueException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidBeginFileException : ILSException
    {
        public InvalidBeginFileException(string m) : base("InvalidBeginFileException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidEndFileException : ILSException
    {
        public InvalidEndFileException(string m) : base("InvalidEndFileException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidEndLineException : ILSException
    {
        public InvalidEndLineException(string m) : base("InvalidEndLineException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidTokenException : ILSException
    {
        public InvalidTokenException(string m) : base("InvalidTokenException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class NoTokenInLineException : ILSException
    {
        public NoTokenInLineException(string m) : base("NoTokenInLineException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidExpressionException : ILSException
    {
        public InvalidExpressionException(string m) : base("InvalidExpression thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidVariableNameException : ILSException
    {
        public InvalidVariableNameException(string m) : base("InvalidVariableNameException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidReturnStatementException : ILSException
    {
        public InvalidReturnStatementException(string m) : base("InvalidReturnStatementException thrown on line " + LinePointer.GetCurrentLine()+": " + m) { }
    }

    class InvalidJumpException : ILSException
    {
        public InvalidJumpException(string m) : base("InvalidJumpException thrown on line " + LinePointer.GetCurrentLine() + ": " + m) { }
    }




}
