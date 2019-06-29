using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class TokenLine
    {
        private List<Token> tokens;
        private readonly string unparsedLine;

        private LineParser parser;
        private LineInterpreter interpreter;

        public TokenLine(string line)
        {
            unparsedLine = line;
            tokens = new List<Token>();

            parser = new LineParser(unparsedLine);
        }

        public void ParseLine()
        {
            if (tokens.Count == 0)
                foreach (Token t in parser.GetTokens())
                    tokens.Add(t);
        }

        public void Interpret()
        {
            if (tokens.Count > 0)
            {
                interpreter = new LineInterpreter(tokens.ToArray());
                interpreter.Interpret();
            }
        }

    }
}
