using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Constants
    {

        public static readonly string BEGINFILE = "ILS!";
        public static readonly string ENDFILE = "SLM!";
        public static readonly string ENDLINE = "next";
        public static readonly string SETVAR = "is";
        public static readonly string VARTYPENUM = "number";
        public static readonly string VARTYPESTR = "string" ;
        public static readonly string[] PUBLICFUNCTIONS = Functions.FUNCTIONNAMES;

        //TODO: Delete this function. Put it in the parser function. Add each type in the type method
        public static TokenType GetType(string str)
        {
            if (str == BEGINFILE)
                return TokenType.BEGINFILE;
            else if (str == ENDFILE)
                return TokenType.ENDFILE;
            else if (str == ENDLINE)
                return TokenType.ENDLINE;
            else if (str == SETVAR)
                return TokenType.SETVAR;
            else if (str == VARTYPENUM)
                return TokenType.VARTYPENUM;
            else if (str == VARTYPESTR)
                return TokenType.VARTYPESTR;
            else if (Array.IndexOf(PUBLICFUNCTIONS, str) != -1)
                return TokenType.FUNCTION;
            else if (IsValidVarName(str))
                return TokenType.VARIABLE;
            else
                return TokenType.UNKNOWN;
        }

        private static bool IsValidValue(string str)
        {
            if (str.Contains('"'))
            {

            }
        }

        private static bool IsValidVarName(string str)
        {

            if (str.Length > 0 && Char.IsLetter(str[0]))
            {
                foreach (char c in str.Substring(1))
                {
                    if (!Char.IsLetterOrDigit(c) && !(c == '_'))
                        return false;
                }

                return true;
            }
            return false;
        }


    }
}
