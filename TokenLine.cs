using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class TokenLine
    {
        private List<Token> tokens;
        private int currentToken = 0;
        public TokenLine(string Line)
        {
            tokens = new List<Token>();

            Parser parser = new Parser(Line);
            parser.Run();

            foreach (string str in parser.GetTokens())
                tokens.Add(new Token(str));
        }

        public Token GetNextToken()
        {
            if (tokens.Count == 0)
                throw new ILSTooFewTokensException("Token Line has no tokens");

            if (currentToken >= tokens.Count-1 && tokens.Count >= 0)
                return null;

            return tokens[currentToken++];
        }

        public Token GetTokenAt(int index)
        {
            return tokens[index];
        }

        public Token GetLastToken()
        {
            return tokens[tokens.Count - 1];
        }

        public int GetNumTokens()
        {
            return tokens.Count-1;
        }

        public bool HasNext()
        {
            return currentToken < tokens.Count-1;
        }
    }
}
