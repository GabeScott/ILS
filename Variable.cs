using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    abstract class Variable
    {
        protected TokenType type = TokenType.VARIABLE;
        protected string name;
        new public TokenType GetType() => type;
        public Variable(string name)
        {
            this.name = name;
        }

        public abstract string GetValAsString();

    }

    class NumberVariable : Variable
    {
        private double val;
        public NumberVariable(string name, double val):base(name)
        {
            this.val = val;
        }

        public double GetVal() => val;

        override public string GetValAsString() => val.ToString();


    }

    class StringVariable : Variable
    {
        
        private string val;
        public StringVariable(string name, string val):base(name)
        {
            this.val = val;
        }
        override public string GetValAsString() => val.ToString();


    }
}
