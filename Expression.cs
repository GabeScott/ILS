using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class Expression
    {

        private readonly List<Token> tokensToEvaluate;

        public Expression(Token[] tokenExpression)
        {
            tokensToEvaluate = new List<Token>(tokenExpression);
        }

        public Token Evaluate()
        {
            ReplaceVariableTokensWithValues();

            EvaluateBasedOnPrecedence(TokenType.HIGH_EXPRESSION);
            EvaluateBasedOnPrecedence(TokenType.LOW_EXPRESSION);

            if (tokensToEvaluate.Count != 1)
                throw new InvalidExpressionException("Invalid number of arguments");

            return tokensToEvaluate[0];
        }


        private void ReplaceVariableTokensWithValues()
        {
            for(int i = 0; i < tokensToEvaluate.Count; i++)
            {
                Token currentToken = tokensToEvaluate[i];
                if (currentToken.IsTypeOf(TokenType.VARIABLE))
                {
                    string variableValue = VariableMap.GetVariableValue(currentToken.ToString());

                    tokensToEvaluate.RemoveAt(i);
                    tokensToEvaluate.Insert(i, new Token(variableValue));
                }
            }
        }


        private void EvaluateBasedOnPrecedence(TokenType precedence)
        {
            int operationIndex = 0;

            while (operationIndex < tokensToEvaluate.Count && !tokensToEvaluate[operationIndex].IsTypeOf(precedence))
                operationIndex++;

            if (operationIndex == tokensToEvaluate.Count)
                return;

            Token evaluation = EvaluateAtIndex(operationIndex);

            tokensToEvaluate.RemoveRange(operationIndex - 1, 3);
            tokensToEvaluate.Insert(operationIndex - 1, evaluation);

            EvaluateBasedOnPrecedence(precedence);
        }


        private Token EvaluateAtIndex(int operationIndex)
        {
            string retval = "";

            string firstVal = tokensToEvaluate[operationIndex - 1].ToString();
            string operation = tokensToEvaluate[operationIndex].ToString();
            string secondVal = tokensToEvaluate[operationIndex + 1].ToString();

            bool firstIsDouble = double.TryParse(firstVal, out double firstresult);
            bool secondIsDouble = double.TryParse(secondVal, out double secondresult);
           
            if (firstIsDouble && secondIsDouble)
                switch (operation)
                {
                    case "/":
                        retval += firstresult / secondresult;
                        break;
                    case "*":
                        retval += firstresult * secondresult;
                        break;
                    case "+":
                        retval += firstresult + secondresult;
                        break;
                    case "-":
                        retval += firstresult - secondresult;
                        break;
                }
            else
            {
                if (operation.ToString() != "+")
                    throw new InvalidTokenException("Invalid string operation: " + operation.ToString());

                retval += firstVal + secondVal;
            }

            return new Token(retval);
        }
    }
}
