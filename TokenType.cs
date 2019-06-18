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
        BEGINLINE,
        ENDLINE,
        SETVAR,
        VARTYPENUM,
        VARTYPESTR,
        FUNCTION,
        VARIABLE,
        VALUE 

    }
}
