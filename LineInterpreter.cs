using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class LineInterpreter
    {

        private List<Token> parsedTokenLine;
        private Token firstToken;

        private int currentTokenIndex = 0;

        private bool containsEndOfFile = false;
        private bool validLine;

        private readonly int VAR_SET_INDEX = 1;
        private readonly int VAR_TYPE_INDEX = 2;
        private readonly int VAR_VAL_INDEX = 3;

        private readonly int FUNC_ARGS = 1;

        public LineInterpreter(Token[] tokens)
        {
            parsedTokenLine = new List<Token>(tokens);
            firstToken = parsedTokenLine[0];

            validLine = IsLineValid();

            containsEndOfFile = CheckIfContainsEndOfFile();
        }


        private bool IsLineValid()
        {
            if (parsedTokenLine.Count == 0)
                return false;

            if (parsedTokenLine.Count == 1 && firstToken.IsTypeOf(TokenType.BEGINFILE, TokenType.ENDFILE))
                return true;

            return true;
        }

        public void Interpret()
        {
            if (!validLine)
                return;

            SimplifyAllExpressions();

            if (firstToken.IsTypeOf(TokenType.VARIABLE))
            {
                CheckVariableSetterTokens();
                AddNewVariable();
            }
            else if (firstToken.IsTypeOf(TokenType.FUNCTION))
                RunFunction();

            if (containsEndOfFile)
                Logger.FoundEndOfFile();
        }


        private void CheckIfLineIsValid()
        {
            if (parsedTokenLine.Count == 1 && firstToken.IsTypeOf(TokenType.BEGINFILE, TokenType.ENDFILE))
                return;

            
        }


        private bool CheckIfContainsEndOfFile()
        {
            foreach (Token t in parsedTokenLine)
                if (t.IsTypeOf(TokenType.ENDFILE))
                    return true;

            return false;
        }



        private void SimplifyAllExpressions()
        {
            int expressionStart, expressionEnd;

            do
            {
                int[] expressionIndexes = FindExpressionIndexes();

                expressionStart = expressionIndexes[0];
                expressionEnd = expressionIndexes[1];

                SimplifyExpression(expressionIndexes[0], expressionIndexes[1]);
            }
            while (expressionStart != -1);

        }


        private int[] FindExpressionIndexes()
        {
            int expressionStart = FindFirstExpressionIndex();
            int expressionEnd = expressionStart;

            if (expressionStart == -1)
                return new int[] { -1, -1 };

            Token currExprToken = parsedTokenLine[expressionStart];

            while (currExprToken.IsExpressionToken() && expressionEnd < parsedTokenLine.Count)
            {
                currExprToken = parsedTokenLine[expressionEnd];
                expressionEnd++;
            }

            if(expressionEnd != parsedTokenLine.Count)
               expressionEnd--;

            return new int[] { expressionStart, expressionEnd };
        }


        private int FindFirstExpressionIndex()
        {
            int index = 0;

            while (index < parsedTokenLine.Count && !parsedTokenLine[index].IsTypeOf(TokenType.HIGH_EXPRESSION, TokenType.LOW_EXPRESSION))
                index++;

            return index == parsedTokenLine.Count ? -1 : index - 1;
        }


        private void SimplifyExpression(int expressionStart, int expressionEnd)
        {
            if (expressionStart == -1)
                return;

            List<Token> tokensToEvaluate = new List<Token>();

            for (int i = expressionStart; i < expressionEnd; i++)
            {
                tokensToEvaluate.Add(parsedTokenLine[expressionStart]);
                parsedTokenLine.RemoveAt(expressionStart);
            }

            Expression expression = new Expression(tokensToEvaluate.ToArray());
            Token evaluatedExpression = expression.Evaluate();

            if (evaluatedExpression != null)
                parsedTokenLine.Insert(expressionStart, evaluatedExpression);
        }


        private void CheckVariableSetterTokens()
        {
            Token setterToken = parsedTokenLine[VAR_SET_INDEX];
            Token varTypeToken = parsedTokenLine[VAR_TYPE_INDEX];
            Token valueToken = parsedTokenLine[VAR_VAL_INDEX];

            if (!setterToken.IsTypeOf(TokenType.SETVAR))
                throw new InvalidTokenException("Expected set variable token, found: " + setterToken);

            if (!varTypeToken.IsTypeOf(TokenType.DECLARENUM, TokenType.DECLARESTR))
                throw new InvalidTokenException("Expected a variable declaration token, found: " + varTypeToken);

            if (!valueToken.IsTypeOf(TokenType.VALUE))
                throw new InvalidTokenException("Expected a value argument token, found: " + valueToken);

        }


        private void AddNewVariable()
        {
            Token varTypeToken = parsedTokenLine[VAR_TYPE_INDEX];
            Token valueToken = parsedTokenLine[VAR_VAL_INDEX];

            string varName = firstToken.ToString();
            string value = valueToken.ToString();

            if (varTypeToken.IsTypeOf(TokenType.DECLARENUM))
                VariableMap.AddNewVariable(varName, GetNumberValue(value));

            else if (varTypeToken.IsTypeOf(TokenType.DECLARESTR))
                VariableMap.AddNewVariable(varName, value);
        }


        private double GetNumberValue(string value)
        {
            double.TryParse(value, out double result);

            if (TokenRules.IsValidNumberValue(value))
                return result;

            throw new InvalidTokenException("Invalid number value: " + value);

        }


        private void RunFunction()
        {
            string functionName = firstToken.ToString();

            List<Token> functionArguments = new List<Token>();

            currentTokenIndex = FUNC_ARGS;

            Token currToken = GetNextToken();

            while (currToken != null)
            {
                functionArguments.Add(currToken);
                currToken = GetNextToken();
            }

            Functions.RunFunction(functionName, functionArguments.ToArray());
        }


        private Token GetNextToken()
        {
            if (currentTokenIndex < parsedTokenLine.Count)
                return parsedTokenLine[currentTokenIndex++];

            else
                return null;
        }
    }
}
