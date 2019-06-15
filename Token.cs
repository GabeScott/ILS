using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    
    class Token
    {
        private string token;
        private TokenType type;

        public Token(string tok)
        {
            token = tok;
            SetType();
        }

        override public string ToString()
        {
            return token;
        }

        public void SetType()
        {
            type = Constants.GetType(token);
        }

        new public TokenType GetType()
        {
            return type;
        }
    }
}
