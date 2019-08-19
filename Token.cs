using System;

namespace ILS
{
    
    public class Token
    {
        private readonly string tokenText;
        private TokenType type;


        public Token(string tok, TokenType type)
        {
            tokenText = tok;
            this.type = type;

            if(IsVagueType())
                SpecifyType();
        }


        override public string ToString()
        {
            return tokenText;
        }


        public bool IsTypeOf(params TokenType[] types)
        {
            return Array.Exists(types, element => element == type);
        }


        private bool IsVagueType()
        {
            return IsTypeOf(TokenType.SINGLE_CHAR, TokenType.KEYWORD, TokenType.UNKNOWN);
        }


        public bool IsExpressionToken()
        {
            return IsTypeOf(TokenType.HIGH_EXPRESSION, TokenType.LOW_EXPRESSION, TokenType.VARIABLE, TokenType.VALUE);
        }


        public bool IsOperationToken()
        {
            return IsTypeOf(TokenType.HIGH_EXPRESSION, TokenType.LOW_EXPRESSION);
        }


        private void SpecifyType()
        {
            if (Constants.ConstantToTokenType.TryGetValue(tokenText, out TokenType tt))
                type = tt;

            else
                type = TokenType.VARIABLE;
        }

    }
}
