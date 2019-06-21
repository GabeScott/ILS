using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Constants
    {
        private static Dictionary<string, TokenType> ConstantToTokenType = new Dictionary<string, TokenType> {
            {  "ILS!", TokenType.BEGINFILE},
            {  "SLM!", TokenType.ENDFILE },
            {  "next" , TokenType.ENDLINE},
            {  "is" , TokenType.SETVAR},
            {  "number" , TokenType.DECLARENUM},
            {  "string" ,TokenType.DECLARESTR}

        };

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

            if (currChar == '\'')
                return IsValidStringLiteral(tokenToTest);
            else if (char.IsDigit(currChar) || currChar == '.')
                return IsValidNumberValue(tokenToTest);

            return false;
        }

        private static bool IsValidStringLiteral(string tokenToTest)
        {
            int index = 1;
            char currChar = tokenToTest[index];

            while (currChar != '\'' && index < tokenToTest.Length)
            {
                currChar = tokenToTest[index];
                index++;
            }

            return currChar == '\'';

        }

        private static bool IsValidNumberValue(string tokenToTest)
        {
            double result;
            return double.TryParse(tokenToTest, out result);
        }




    }
}
