using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    enum TokenType
    {
        UNKNOWN,
        BEGINFILE,
        ENDFILE,
        ENDLINE,
        SETVAR,
        DECLARENUM,
        DECLARESTR,
        FUNCTION,
        VARIABLE,
        VARIABLESTR,
        VARIABLENUM,
        VALUE,
        HIGH_EXPRESSION,
        LOW_EXPRESSION

    }
}
