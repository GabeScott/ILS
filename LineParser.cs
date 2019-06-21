using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class LineParser
    {

        private string unparsedLine;
        private char[] unparsedLineCharArray;

        private List<Token> tokensFoundInLine;

        private int beginTokenIndex = 0;
        private int endTokenIndex = 0;

        private int lineNumberInFile;

        private bool inputHasBeenParsed = false;

        public LineParser(string unparsedLine, int lineNumberInFile)
        {
            this.unparsedLine = unparsedLine;
            this.lineNumberInFile = lineNumberInFile;

            unparsedLineCharArray = unparsedLine.ToCharArray();
            tokensFoundInLine = new List<Token>();
        }


        public Token[] GetTokens()
        {
            if (!inputHasBeenParsed)
                LoadInputAndParse();

            return tokensFoundInLine.ToArray();
        }

        private void LoadInputAndParse()
        {

            while (beginTokenIndex < unparsedLineCharArray.Length)
            {
                char currChar = unparsedLineCharArray[endTokenIndex];
                 
                if (char.IsDigit(currChar) || currChar == '.')
                    GetNumberValue();

                else if (char.IsLetter(currChar))
                    GetKeyword();

                else if (currChar == '\'')
                    GetStringLiteral();
            }

            inputHasBeenParsed = true;
        }

        private void GetNumberValue()
        {
            int numDecimals = 0;
            char currChar;
            bool validDecimalNumber = false;

            do
            {
                currChar = unparsedLineCharArray[endTokenIndex];

                if (currChar == '.')
                    numDecimals++;
                
               validDecimalNumber = (currChar == '.') && numDecimals <= 1;

                endTokenIndex++;
            }
            while ((char.IsDigit(currChar) || validDecimalNumber) && endTokenIndex < unparsedLineCharArray.Length);

            if (currChar != ' ')
                throw new InvalidTokenException("Invalid token: " + GetTokenFromIndexes() + " on line number " + lineNumberInFile);

            tokensFoundInLine.Add(new Token(GetTokenFromIndexes(), TokenType.VALUE));

            beginTokenIndex = endTokenIndex;
        }


        private string GetTokenFromIndexes()
        {
            return unparsedLine.Substring(beginTokenIndex, endTokenIndex - beginTokenIndex);
        }



        private bool IsValidKeywordChar(char c)
        {
            return (char.IsLetterOrDigit(c) || c == '_' || c == '!') && c != ' ';
        }


        private void GetKeyword()
        {
            char currChar = unparsedLineCharArray[endTokenIndex];
            endTokenIndex++;

            while (IsValidKeywordChar(currChar) && endTokenIndex < unparsedLineCharArray.Length)
            {

                currChar = unparsedLineCharArray[endTokenIndex];
                endTokenIndex++;
            }


            if (currChar != ' ' && endTokenIndex < unparsedLineCharArray.Length)
                throw new InvalidTokenException("Invalid token");


            string tokenContents;

            if (endTokenIndex == unparsedLineCharArray.Length)
                tokenContents = unparsedLine.Substring(beginTokenIndex, endTokenIndex - beginTokenIndex);
            else
                tokenContents = unparsedLine.Substring(beginTokenIndex, endTokenIndex - beginTokenIndex - 1);


            tokensFoundInLine.Add(new Token(tokenContents, Constants.GetTokenTypeByConstant(tokenContents)));

            beginTokenIndex = endTokenIndex;

        }

        private void GetStringLiteral()
        {
            char targetChar = unparsedLineCharArray[endTokenIndex];

            endTokenIndex++;
            char currChar = unparsedLineCharArray[endTokenIndex];

            while (currChar != targetChar && endTokenIndex < unparsedLineCharArray.Length)
            {
                currChar = unparsedLineCharArray[endTokenIndex];
                endTokenIndex++;
            }

            if (endTokenIndex == unparsedLineCharArray.Length)
                throw new TooFewTokensException("Reached end of line while parsing");


            char spaceAfterToken = unparsedLineCharArray[endTokenIndex];

            if (spaceAfterToken != ' ' && endTokenIndex < unparsedLineCharArray.Length)
                throw new InvalidTokenException("Invalid token");

            string tokenContents = unparsedLine.Substring(beginTokenIndex, endTokenIndex - beginTokenIndex);
            tokensFoundInLine.Add(new Token(tokenContents, TokenType.VALUE));


            beginTokenIndex = ++endTokenIndex;
        }
    }
}
