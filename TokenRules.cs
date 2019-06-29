using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class TokenRules
    {

        public static bool IsValidValue(string tokenToTest)
        {
            return IsValidStringLiteral(tokenToTest) || IsValidNumberValue(tokenToTest);
        }

        public static bool IsValidKeyword(string tokenToTest)
        {
            if (tokenToTest.Length == 0)
                return false;

            char firstLetter = tokenToTest[0];

            if (!char.IsLetter(firstLetter))
                return false;

            string restOfLetters = tokenToTest.Substring(1);

            foreach (char c in restOfLetters)
                if (!char.IsLetterOrDigit(c) && c != '_' && c != '!')
                    return false;

            return true;
        }

        public static bool IsValidKeywordStart(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        public static bool IsValidStringLiteral(string tokenToTest)
        {
            if (tokenToTest.Length < 2)
                return false;

            char stringLiteralStart = tokenToTest[0];

            if (stringLiteralStart != Constants.stringLiteralIdentifier)
                return false;

            int index = 1;
            char currChar;

            do
            {
                currChar = tokenToTest[index];
                index++;
            }
            while (currChar != stringLiteralStart && index < tokenToTest.Length);


            return currChar == stringLiteralStart;

        }

        public static bool IsValidStringLiteralStart(char c)
        {
            return c == Constants.stringLiteralIdentifier;
        }

        public static bool IsNotStringLiteralIdentifier(string tokenToTest)
        {
            return tokenToTest[tokenToTest.Length - 1] != Constants.stringLiteralIdentifier;
        }

        public static bool IsValidNumberValue(string tokenToTest)
        {
            if (tokenToTest == ".")
                return false;

            foreach (char c in tokenToTest)
                if (!char.IsDigit(c) && c != Constants.decimalPoint)
                    return false;


            return double.TryParse(tokenToTest, out double result);
        }

        public static bool IsValidNumStart(char c)
        {
            return char.IsDigit(c) || c == Constants.decimalPoint;
        }

        public static bool IsValidHighExpression(string tokenToTest)
        {
            if (tokenToTest.Length > 1)
                return false;

            char token = tokenToTest[0];

            return Array.Exists(Constants.validHighExpressions, element => element == token);
        }

        public static bool IsValidLowExpression(string tokenToTest)
        {
            if (tokenToTest.Length > 1)
                return false;

            char token = tokenToTest[0];

            return Array.Exists(Constants.validLowExpressions, element => element == token);
        }

        public static bool IsValidCharAfterToken(char tokenToTest)
        {
            return Array.Exists(Constants.validCharsAfterToken, element => element == tokenToTest);
        }

        public static bool IsValidFunction(string tokenToTest)
        {
            foreach (FunctionName fn in Enum.GetValues(typeof(FunctionName)))
            {
                if (tokenToTest == fn.ToString())
                    return true;
            }

            return false;
        }



    }
}
