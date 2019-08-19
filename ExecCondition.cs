using System.Collections.Generic;


namespace ILS
{
    class ExecCondition
    {
        List<Token> conditionTokens;
        private bool okToExecute;

        public ExecCondition(Token[] tokens)
        {
            conditionTokens = new List<Token>(tokens);

            if (tokens.Length > 0)
                EvaluateCondition();
            else
                okToExecute = true;
        }


        public bool OkToExecute()
        {
            return okToExecute;
        }


        private void EvaluateCondition()
        {
            if (conditionTokens.Count != 3)
                throw new ILSException("Invalid number of conditional arguments. Expected 3, found " + conditionTokens.Count);

            ReplaceVariables();

            Token firstOperand = conditionTokens[0];
            Token operation = conditionTokens[1];
            Token secondOperand = conditionTokens[2];

            bool firstIsNum = double.TryParse(firstOperand.ToString(), out double firstResult);
            bool secondIsNum = double.TryParse(secondOperand.ToString(), out double secondResult);

            if (firstIsNum && secondIsNum)
                switch (operation.ToString())
                {
                    case "<":
                        okToExecute = firstResult < secondResult;
                        break;
                    case ">":
                        okToExecute = firstResult > secondResult;
                        break;
                    case "=":
                        okToExecute = firstResult == secondResult;
                        break;
                    case "#":
                        okToExecute = firstResult != secondResult;
                        break;
                    default:
                        throw new ILSException("Invalid condition operation: " + operation);
                }

            else
                switch (operation.ToString())
                {
                    case "=":
                        okToExecute = firstOperand.ToString() == secondOperand.ToString();
                        break;
                    case "#":
                        okToExecute = firstOperand.ToString() != secondOperand.ToString();
                        break;
                    default:
                        throw new ILSException("Invalid string condition operation: " + operation);
                }

        }
        



        private void ReplaceVariables()
        {
            for(int i = 0; i < conditionTokens.Count; i++)
            {
                Token currToken = conditionTokens[i];
                if (currToken.IsTypeOf(TokenType.VARIABLE))
                {
                    Token valueToken = new Token(VariableMap.GetVariableValue(currToken.ToString()), TokenType.VALUE);
                    conditionTokens.RemoveAt(i);
                    conditionTokens.Insert(i, valueToken);
                }

            }
        }

    }
}
