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
            if (currentToken >= tokens.Count || tokens.Count == 0)
                return null;

            return tokens[currentToken++];
        }



        public Token GetTokenAt(int index)
        {
            if (index >= tokens.Count)
                return null;

            return tokens[index];
        }

        public bool ContainsEndOfFile() => tokens.Exists(element => element.Type == TokenType.ENDFILE);
        public Token GetCurrentToken() => tokens[currentToken];

        public Token GetLastToken() => tokens[tokens.Count - 1];

        public int GetNumTokens() => tokens.Count - 1;

        public bool IsEmpty() => tokens.Count == 0;

        public bool HasNext() => currentToken < tokens.Count;

    }
}
