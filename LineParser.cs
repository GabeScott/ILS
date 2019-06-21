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
        private bool foundEndOfFile = false;

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
            char currChar;

            while (beginTokenIndex < unparsedLineCharArray.Length)
            {
                currChar = unparsedLineCharArray[endTokenIndex];
                 
                if (char.IsDigit(currChar) || currChar == Constants.decimalPoint)
                    SetNumberValueIndexes();

                else if (char.IsLetter(currChar))
                    SetKeywordIndexes();

                else if (Constants.IsValidStringLiteralStart(currChar))
                    SetStringLiteralIndexes();

                AddCurrentToken();
                UpdateTokenIndexes();
                CheckForEndOfFileToken();
            }

            inputHasBeenParsed = true;
        }

        private void SetNumberValueIndexes()
        {
            int numDecimals = 0;
            char currChar;
            bool validDecimalNumber = false;

            do
            {
                currChar = unparsedLineCharArray[endTokenIndex];

                if (currChar == Constants.decimalPoint)
                    numDecimals++;
                
               validDecimalNumber = (currChar == Constants.decimalPoint) && numDecimals <= 1;

                endTokenIndex++;
            }
            while ((char.IsDigit(currChar) || validDecimalNumber) && endTokenIndex < unparsedLineCharArray.Length);

            if (currChar != Constants.emptySpaceAfterToken)
                throw new InvalidTokenException("Invalid token: " + GetTokenFromIndexes() + " on line number " + lineNumberInFile);

            endTokenIndex--;
        }


        private string GetTokenFromIndexes()
        {
            return unparsedLine.Substring(beginTokenIndex, endTokenIndex - beginTokenIndex);
        }


        private void SetKeywordIndexes()
        {
            char currChar;

            do
            {
                currChar = unparsedLineCharArray[endTokenIndex];
                endTokenIndex++;
            }
            while (IsValidKeywordChar(currChar) && endTokenIndex < unparsedLineCharArray.Length);


            if (currChar != Constants.emptySpaceAfterToken && endTokenIndex < unparsedLineCharArray.Length)
                throw new InvalidTokenException("Invalid token on line " + lineNumberInFile);

            endTokenIndex = (endTokenIndex == unparsedLineCharArray.Length) ? endTokenIndex : --endTokenIndex;


        }

        private bool IsValidKeywordChar(char c)
        {
            return (char.IsLetterOrDigit(c) || c == '_' || c == '!') && c != ' ';
        }

        private void SetStringLiteralIndexes()
        {
            char singleOrDoubleQuote = unparsedLineCharArray[endTokenIndex];

            endTokenIndex++;
            char currChar;

            do
            {
                currChar = unparsedLineCharArray[endTokenIndex];
                endTokenIndex++;
            }
            while (currChar != singleOrDoubleQuote && endTokenIndex < unparsedLineCharArray.Length);


            if (endTokenIndex == unparsedLineCharArray.Length)
                throw new TooFewTokensException("Reached end of line while parsing on line " + lineNumberInFile);

            char shouldBeEmptySpace = unparsedLineCharArray[endTokenIndex];

            if (shouldBeEmptySpace != Constants.emptySpaceAfterToken)
                throw new InvalidTokenException("Invalid token on line " + lineNumberInFile);

        }

        private void AddCurrentToken()
        {
            string tokenToAdd = GetTokenFromIndexes();
            tokensFoundInLine.Add(new Token(tokenToAdd, Constants.GetTokenTypeByConstant(tokenToAdd)));
        }

        private void UpdateTokenIndexes()
        {
            beginTokenIndex = ++endTokenIndex;
        }

        private void CheckForEndOfFileToken()
        {
            if (tokensFoundInLine[tokensFoundInLine.Count - 1].Type == TokenType.ENDFILE)
            {
                foundEndOfFile = true;
                beginTokenIndex = unparsedLineCharArray.Length;
            }
        }
    }
}
