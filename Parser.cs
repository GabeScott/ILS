using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class Parser
    {
        private string line;
        private char[] lineArray;
        private List<string> tokens;
        private TokenType currentType = TokenType.BEGINLINE;
        private int tokenBegin = 0, tokenEnd = 0;
        public Parser(string s)
        {
            line = s;
            lineArray = s.ToCharArray();
            tokens = new List<string>();
        }

        public void Run()
        {
            if (line == "ILS!" || line == "SLM!")
            {
                tokens.Add(line);
                return;
            }


            while (tokenBegin < lineArray.Length)
            {
                char currChar = lineArray[tokenEnd];

                if (currChar == '.')
                    GetValueStartingAtDecimal();

                else if (Char.IsDigit(currChar))
                    GetValueStartingAtDigit();

                else if (Char.IsLetter(currChar))
                    GetKeyword();

                else if (currChar == '"')
                    GetStringLiteral();
            }


        }

        public string[] GetTokens()
        {
            return tokens.ToArray();
        }


        private void GetValueStartingAtDecimal()
        {
            tokenEnd++;
            char currChar = lineArray[tokenEnd];

            while (Char.IsDigit(currChar))
            {
                tokenEnd++;
                currChar = lineArray[tokenEnd];
            }

            if (currChar != ' ')
                throw new InvalidTokenException("Invalid token");

            tokens.Add(line.Substring(tokenBegin, tokenEnd - tokenBegin));

            tokenEnd++;
            tokenBegin = tokenEnd;

        }

        private void GetValueStartingAtDigit()
        {
            tokenEnd++;
            char currChar = lineArray[tokenEnd];
            bool hitDecimal = false;

            while (Char.IsDigit(currChar) || (currChar == '.' && !hitDecimal))
            {
                hitDecimal = hitDecimal || (currChar == '.');

                tokenEnd++;
                currChar = lineArray[tokenEnd];
            }

            if (currChar != ' ')
                throw new InvalidTokenException("Invalid token");

            tokens.Add(line.Substring(tokenBegin, tokenEnd - tokenBegin));

            tokenEnd++;
            tokenBegin = tokenEnd;
        }

        private bool IsValidKeywordChar(char c)
        {
            return ((Char.IsLetterOrDigit(c) || c == '_') && c != ' ');
        }
        private void GetKeyword()
        {
            char currChar = lineArray[tokenEnd];
            tokenEnd++;


            while (IsValidKeywordChar(currChar) && tokenEnd < lineArray.Length)
            {
                
                currChar = lineArray[tokenEnd];
                tokenEnd++;
            }


            if (currChar != ' ' && tokenEnd < lineArray.Length)
                throw new InvalidTokenException("Invalid token");

            if(tokenEnd == lineArray.Length)
                tokens.Add(line.Substring(tokenBegin, tokenEnd - tokenBegin));

            else
                tokens.Add(line.Substring(tokenBegin, tokenEnd - tokenBegin - 1));

            tokenBegin = tokenEnd;
            
        }

        private void GetStringLiteral()
        {
            tokenEnd++;
            char currChar = lineArray[tokenEnd];

            while (currChar != '"')
            {
                if(currChar == '\n')
                    throw new InvalidTokenException("Unexpected end of line");

                tokenEnd++;
                currChar = lineArray[tokenEnd];
            }

            if (currChar != ' ')
                throw new InvalidTokenException("Invalid token");

            tokens.Add(line.Substring(tokenBegin, tokenEnd - tokenBegin));

            tokenEnd++;
            tokenBegin = tokenEnd;
        }

        

    }
}
