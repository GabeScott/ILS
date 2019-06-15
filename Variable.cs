using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    abstract class Variable
    {

        protected string name;
        new public abstract TokenType GetType();
        public Variable(string name)
        {
            this.name = name;
        }

        public abstract string GetValAsString();

    }

    class ILSVariableNum : Variable
    {
        private TokenType type = TokenType.VARIABLE;
        private double val;
        public ILSVariableNum(string name, double val):base(name)
        {
            this.val = val;
        }

        override public TokenType GetType()
        {
            return type;
        }

        public string GetVal()
        {
            return val.ToString();
        }

        override public string GetValAsString()
        {
            return val.ToString();
        }


    }

    class ILSVariableStr : Variable
    {
        private TokenType type = TokenType.VARIABLE;
        private string val;
        public ILSVariableStr(string name, string val):base(name)
        {
            this.val = val;
        }

        override public TokenType GetType()
        {
            return type;
        }

        public string GetVal()
        {
            return val;
        }

        override public string GetValAsString()
        {
            return val;
        }


    }
}
