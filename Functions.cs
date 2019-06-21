using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Functions
    {
        public enum FunctionName
        {
            PRINT,
            PRINTN
        }



        public static bool IsValidFunction(string tokenToTest)
        {
            foreach (FunctionName fn in Enum.GetValues(typeof(FunctionName)))
            {
                if (Enum.TryParse(tokenToTest, out FunctionName result))
                    return true;
            }

            return false;
        }

        public static void RunFunction(string function, Token[] functionArguments)
        {
            string output = GetOutput(functionArguments);

            FunctionName result;
            Enum.TryParse(function, out result);

            if (result == FunctionName.PRINT)
                Console.Write(output);

            else if (result == FunctionName.PRINTN)
                Console.WriteLine(output);

        }

        private static string GetOutput(Token[] functionArguments)
        {
            string output = "";

            foreach (Token t in functionArguments)
                if (t.Type == TokenType.VARIABLE)
                    output += VariableMap.GetVarByName(t.ToString()).GetValAsString();
                else
                    output += t.ToString();

            return CleanStringLiteral(output);
        }

        private static string CleanStringLiteral(string stringLiteral)
        {
            string cleanedString = "";

            foreach (char c in stringLiteral)
                if (!Constants.IsValidStringLiteralStart(c))
                    cleanedString += c;

            return cleanedString;
        }

    }
}
