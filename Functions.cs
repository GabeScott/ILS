using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Functions
    {

        public static readonly string[] FUNCTIONNAMES = { "PRINT", "PRINTN" };

        public static void RunFunction(string function, Token[] paramTokens)
        {
            if (function == FUNCTIONNAMES[0])
                RunPrint(paramTokens, false);
            else if (function == FUNCTIONNAMES[1])
                RunPrint(paramTokens, true);

        }

        private static void RunPrint(Token[] paramTokens, bool newline) {
            string s = "";

            foreach (Token i in paramTokens)
            {
                if (i.GetType() == TokenType.VARIABLE)
                    s += VariableMap.GetVarByName(i.ToString()).GetValAsString();
                else
                    s += i.ToString();
            }

            if (newline)
                Console.WriteLine(s);
            else
                Console.Write(s);
        }

    }
}
