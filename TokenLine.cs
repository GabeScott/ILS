using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class TokenLine
    {
        private List<Token> tokens;
        private int currentToken = 0;
        public int LineNumberInFile {get; }
        public TokenLine(string line, int linenum)
        {
            LineNumberInFile = linenum;

            tokens = new List<Token>();

            LineParser parser = new LineParser(line, LineNumberInFile);

            foreach (Token t in parser.GetTokens())
                tokens.Add(t);
        }

        public Token GetNextToken()
        {
            if (currentToken >= tokens.Count - 1 || tokens.Count == 0)
                return null;

            return tokens[currentToken++];
        }

        public Token GetCurrentToken()
        {
            return tokens[currentToken-1];
        }

        public Token GetTokenAt(int index)
        {
            if (index >= tokens.Count)
                return null;

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

        public bool IsEmpty()
        {
            return tokens.Count == 0;
        }

        public bool HasNext()
        {
            return currentToken < tokens.Count-1;
        }

    }
}
