using System.Collections.Generic;

namespace ILS
{
    public enum TokenType
    {
        UNKNOWN,
        BEGINFILE,
        ENDFILE,
        KEYWORD,
        SINGLE_CHAR,
        SETVAR,
        DECLARENUM,
        DECLARESTR,
        FUNCTION,
        VARIABLE,
        VARIABLESTR,
        VARIABLENUM,
        VALUE,
        HIGH_EXPRESSION,
        LOW_EXPRESSION,
        COND_EXPRESSION,
        COND_START,
        LINE_MARKER
    }


    static class Constants
    {
        public static readonly string beginFileToken = "ILS!";
        public static char stringLiteralIdentifier = '\'';

        public static Dictionary<string, TokenType> ConstantToTokenType = new Dictionary<string, TokenType> {
            {  "ILS!", TokenType.BEGINFILE },
            {  "SLM!", TokenType.ENDFILE },
            {  "is" , TokenType.SETVAR },
            {  "number" , TokenType.DECLARENUM },
            {  "string" ,TokenType.DECLARESTR },
            {  "PRINT" ,TokenType.FUNCTION },
            {  "PRINTN" ,TokenType.FUNCTION },
            {  "UPDATE" ,TokenType.FUNCTION },
            {  "JUMPTO" ,TokenType.FUNCTION },
            {  "JUMPONCE" ,TokenType.FUNCTION },
            {  "RETURN" ,TokenType.FUNCTION },
            {  "DELETE" ,TokenType.FUNCTION },
            {  "+" ,TokenType.LOW_EXPRESSION },
            {  "-" ,TokenType.LOW_EXPRESSION },
            {  "*" ,TokenType.HIGH_EXPRESSION },
            {  "/" ,TokenType.HIGH_EXPRESSION },
            {  "?" ,TokenType.COND_START },
            {  ">" ,TokenType.COND_EXPRESSION },
            {  "<" ,TokenType.COND_EXPRESSION },
            {  "=" ,TokenType.COND_EXPRESSION },
            {  "#" ,TokenType.COND_EXPRESSION }
        };
    }
}
