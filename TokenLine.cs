using System.Collections.Generic;

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

            parser = new LineParser(unparsedLine);
            tokens = new List<Token>(parser.GetTokens());
        }

        public void ParseAndExecute()
        {
            if (tokens.Count > 0)
            {
                interpreter = new LineInterpreter(tokens.ToArray());
                interpreter.Interpret();
            }
        }



        public void CheckForLineMarker(int line)
        {
            if (tokens.Count > 0 && tokens[0].IsTypeOf(TokenType.LINE_MARKER))
            {
                LineMarkers.AddLineMarker(tokens[0].ToString().Substring(1), line);
                tokens.RemoveAt(0);
            }
        }

    }
}
