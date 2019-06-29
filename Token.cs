using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    
    class Token
    {
        private readonly string tokenText;
        private TokenType type;


        public Token(string tok)
        {
            tokenText = tok;
            SetTokenType();
        }


        override public string ToString()
        {
            return tokenText;
        }


        public bool IsTypeOf(params TokenType[] types)
        {
            return Array.Exists(types, element => element == type);
        }


        public bool IsExpressionToken()
        {
            return IsTypeOf(TokenType.HIGH_EXPRESSION, TokenType.LOW_EXPRESSION, TokenType.VARIABLE, TokenType.VALUE);
        }


        private void SetTokenType()
        {
            if (Constants.ConstantToTokenType.ContainsKey(tokenText))
                type = Constants.ConstantToTokenType.GetValueOrDefault(tokenText);

            else if (TokenRules.IsValidFunction(tokenText))
                type = TokenType.FUNCTION;

            else if (TokenRules.IsValidKeyword(tokenText))
                type = TokenType.VARIABLE;

            else if (TokenRules.IsValidValue(tokenText))
                type = TokenType.VALUE;

            else if (TokenRules.IsValidHighExpression(tokenText))
                type = TokenType.HIGH_EXPRESSION;

            else if (TokenRules.IsValidLowExpression(tokenText))
                type = TokenType.LOW_EXPRESSION;

            else
                type = TokenType.UNKNOWN;
        }
    }
}
