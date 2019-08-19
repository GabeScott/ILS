
using System.Collections.Generic;

namespace ILS
{
    public class LineParser
    {
        private string lineToParse;
        private List<Token> tokensFoundInLine;


        public LineParser(string lineToParse)
        {
            this.lineToParse = lineToParse;
            tokensFoundInLine = new List<Token>();
            RunParse();
        }


        private void RunParse()
        {
            StateTransitioner stateTransitioner = new StateTransitioner();

            string currentToken = "";
            TokenType currentTokenType;


            foreach(char currChar in lineToParse)
            {
                currentTokenType = stateTransitioner.GetCurrentTokenType();

                stateTransitioner.UpdateState(currChar);

                if (stateTransitioner.IsMultiCharacterToken())
                {
                    tokensFoundInLine.Add(new Token(currentToken, currentTokenType));
                    tokensFoundInLine.Add(new Token(currChar.ToString(), TokenType.SINGLE_CHAR));

                    stateTransitioner.ResetState();
                    currentToken = "";
                }

                else if (stateTransitioner.IsSingleCharacterToken())
                {
                    currentToken += currChar;
                    tokensFoundInLine.Add(new Token(currentToken, currentTokenType));

                    stateTransitioner.ResetState();
                    currentToken = "";
                }

                else if (stateTransitioner.IsInvalidState())
                    throw new ILSException("Invalid token: " + currentToken + currChar);

                else
                    currentToken += currChar;
            }

            if (currentToken.Length > 0)
            {
                currentTokenType = stateTransitioner.GetCurrentTokenType();
                tokensFoundInLine.Add(new Token(currentToken, currentTokenType));
            }
            RemoveEmptyTokens();
        }


        private void RemoveEmptyTokens()
        {
            for (int i = 0; i < tokensFoundInLine.Count; i++)
                if (tokensFoundInLine[i].ToString() == " ")
                {
                    tokensFoundInLine.RemoveAt(i);
                    i--;
                }
        }



        public Token[] GetTokens()
        {
            return tokensFoundInLine.ToArray();
        }

    }
}
