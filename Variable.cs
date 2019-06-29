using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    abstract class Variable
    {
        public string Name { get; }
        public new abstract TokenType GetType();
        public Variable(string name)
        {
            Name = name;
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
        override public TokenType GetType() => TokenType.VARIABLENUM;

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

        override public TokenType GetType() => TokenType.VARIABLESTR;



    }
}
