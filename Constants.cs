using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class Constants
    {
        
        public static char decimalPoint = '.';
        public static char stringLiteralIdentifier = '\'';

        public static char[] validCharsAfterToken = new char[] { ' ', '+', '-', '/', '*' };
        public static char[] validHighExpressions = new char[] {'/', '*' };
        public static char[] validLowExpressions = new char[] { '+', '-'};

        public static Dictionary<string, TokenType> ConstantToTokenType = new Dictionary<string, TokenType> {
            {  "ILS!", TokenType.BEGINFILE},
            {  "SLM!", TokenType.ENDFILE },
            {  "is" , TokenType.SETVAR},
            {  "number" , TokenType.DECLARENUM},
            {  "string" ,TokenType.DECLARESTR}

        };
    }
}
