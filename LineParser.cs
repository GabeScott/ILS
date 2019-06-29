using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class LineParser
    {
        private delegate bool TokenRule(string tokenToTest);

        private string unparsedLine;
        private char[] unparsedLineCharArray;

        private List<Token> tokensFoundInLine;

        private int beginTokenIndex = 0;
        private int endTokenIndex = 0;

        private bool inputHasBeenParsed = false;


        public LineParser(string unparsedLine)
        {
            this.unparsedLine = unparsedLine;

            unparsedLineCharArray = unparsedLine.ToCharArray();
            tokensFoundInLine = new List<Token>();
        }


        public Token[] GetTokens()
        {
            if (!inputHasBeenParsed)
                Parse();

            return tokensFoundInLine.ToArray();
        }

        private void Parse()
        {
            char currChar;

            while (beginTokenIndex < unparsedLineCharArray.Length)
            {
                currChar = unparsedLineCharArray[endTokenIndex];

                if (TokenRules.IsValidNumStart(currChar))
                    SetIndexesBasedOnRule(TokenRules.IsValidNumberValue);

                else if (TokenRules.IsValidKeywordStart(currChar))
                    SetIndexesBasedOnRule(TokenRules.IsValidKeyword);

                else if (TokenRules.IsValidStringLiteralStart(currChar))
                {
                    endTokenIndex++;
                    SetIndexesBasedOnRule(TokenRules.IsNotStringLiteralIdentifier);
                }

                else if (TokenRules.IsValidHighExpression(currChar.ToString())
                             || TokenRules.IsValidLowExpression(currChar.ToString()))
                    endTokenIndex++;

                else
                    beginTokenIndex = ++endTokenIndex;

                AddCurrentToken();
                UpdateTokenIndexes();
            }

            inputHasBeenParsed = true;
        }


        private void SetIndexesBasedOnRule(TokenRule tokenRule)
        {
            
            char currChar;
            string currentWord = "";

            do
            {
                currChar = unparsedLineCharArray[endTokenIndex];
                currentWord += currChar;
                endTokenIndex++;
            }
            while (tokenRule(currentWord) && endTokenIndex < unparsedLineCharArray.Length);

            HandleLastCharacter(currChar);
        }


        private void HandleLastCharacter(char endingChar)
        {
            bool requiresLastCharacter = endTokenIndex == unparsedLineCharArray.Length || !TokenRules.IsValidCharAfterToken(endingChar);

            if (!requiresLastCharacter)
                endTokenIndex--;

            if (endTokenIndex == unparsedLineCharArray.Length)
                return;

            char afterToken = unparsedLineCharArray[endTokenIndex];

            if (!TokenRules.IsValidCharAfterToken(afterToken))
                throw new InvalidTokenException("Invalid space after token: " + afterToken);

        }


        private void AddCurrentToken()
        {
            string tokenToAdd = GetTokenFromIndexes();

            if(tokenToAdd != null)
                tokensFoundInLine.Add(new Token(tokenToAdd));
        }


        private string GetTokenFromIndexes()
        {
            if (beginTokenIndex == endTokenIndex)
                return null;

            return unparsedLine.Substring(beginTokenIndex, endTokenIndex - beginTokenIndex);
        }


        private void UpdateTokenIndexes()
        {
            beginTokenIndex = endTokenIndex;
        }
    }
}
