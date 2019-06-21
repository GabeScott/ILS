using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    
    class Token
    {
        private string token;
        public TokenType Type { get; }

        public Token(string tok)
        {
            token = tok;
            Type = TokenType.UNKNOWN;
        }

        public Token(string tok, TokenType tt)
        {
            token = tok;
            Type = tt;
        }

        override public string ToString() => token;

        public bool IsTypeOf(TokenType tt) => Type == tt;
    }
}
