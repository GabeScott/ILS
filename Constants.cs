using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Constants
    {
        public static char emptySpaceAfterToken = ' ';
        public static char decimalPoint = '.';

        private static Dictionary<string, TokenType> ConstantToTokenType = new Dictionary<string, TokenType> {
            {  "ILS!", TokenType.BEGINFILE},
            {  "SLM!", TokenType.ENDFILE },
            {  "next" , TokenType.ENDLINE},
            {  "is" , TokenType.SETVAR},
            {  "number" , TokenType.DECLARENUM},
            {  "string" ,TokenType.DECLARESTR}

        };

        private static char[] stringLiteralIdentifiers = new char[] { '\'', '"'};

        public static string GetConstantByTokenType(TokenType tokenTypeToTest)
        {
            foreach (string s in ConstantToTokenType.Keys)
                if (tokenTypeToTest == ConstantToTokenType.GetValueOrDefault(s))
                    return s;

            return null;
        }

        public static TokenType GetTokenTypeByConstant(string tokenToTest)
        {
            if (ConstantToTokenType.ContainsKey(tokenToTest))
                return ConstantToTokenType.GetValueOrDefault(tokenToTest);

            else if (Functions.IsValidFunction(tokenToTest))
                return TokenType.FUNCTION;
            else if (IsValidVarName(tokenToTest))
                return TokenType.VARIABLE;
            else if (IsValidValue(tokenToTest))
                return TokenType.VALUE;
            else
                return TokenType.UNKNOWN;
        }

        public static bool IsValidStringLiteralStart(char charToTest)
        {
            return Array.Exists(stringLiteralIdentifiers, element => element == charToTest);
        }

        private static bool IsValidVarName(string tokenToTest)
        {

            if (tokenToTest.Length == 0)
                return false;

            char firstLetter = tokenToTest[0];

            if (!char.IsLetter(firstLetter))
                return false;

            string restOfLetters = tokenToTest.Substring(1);

            foreach (char c in restOfLetters)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return false;
            }

            return true;

        }


        private static bool IsValidValue(string tokenToTest)
        {
            int index = 0;
            char currChar = tokenToTest[index];

            if (IsValidStringLiteralStart(currChar))
                return IsValidStringLiteral(tokenToTest);
            else if (char.IsDigit(currChar) || currChar == '.')
                return IsValidNumberValue(tokenToTest);

            return false;
        }

        private static bool IsValidStringLiteral(string tokenToTest)
        {
            char stringLiteralStart = tokenToTest[0];

            int index = 1;
            char currChar;

            do
            {
                currChar = tokenToTest[index];
                index++;
            }
            while (!IsValidStringLiteralStart(currChar) && index < tokenToTest.Length);


            return currChar == stringLiteralStart;

        }

        private static bool IsValidNumberValue(string tokenToTest)
        {
            double result;
            return double.TryParse(tokenToTest, out result);
        }




    }
}
