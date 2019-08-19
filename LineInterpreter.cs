using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    class LineInterpreter
    {

        private List<Token> parsedTokenLine;
        private List<Token> conditionalTokens;
        private Token firstToken;

        private bool containsEndOfFile = false;

        private readonly int VAR_SET_INDEX = 1;
        private readonly int VAR_TYPE_INDEX = 2;
        private readonly int VAR_VAL_INDEX = 3;

        private readonly int FUNCTION_NAME = 0;
        private readonly int FUNC_ARGS = 1;

        private int NO_EXPRESSION;

        public LineInterpreter(Token[] tokens)
        {
            conditionalTokens = new List<Token>();
            parsedTokenLine = new List<Token>(tokens);

            firstToken = parsedTokenLine[0];

            containsEndOfFile = CheckIfContainsEndOfFile();

            SimplifyAllExpressions();

            if(firstToken.IsTypeOf(TokenType.COND_START))
                AddConditionalTokens();

            NO_EXPRESSION = parsedTokenLine.Count - 1;
        }

        public void Interpret()
        {

            if (!SimplifyConditionalExpression())
                return;

            
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


        private bool CheckIfContainsEndOfFile()
        {
            return Array.Exists(parsedTokenLine.ToArray(), element => element.IsTypeOf(TokenType.ENDFILE));
        }


        private void AddConditionalTokens()
        {
            int index = 1;

            while (index < parsedTokenLine.Count)
            {
                Token t = parsedTokenLine[index];

                if (t.IsTypeOf(TokenType.COND_START))
                    break;

                conditionalTokens.Add(t);
                index++;
            }

            if (conditionalTokens.Count == 0)
                throw new ILSException("Invalid conditional statement, no tokens found");

            RemoveConditionalTokens(index);

            if (parsedTokenLine.Count == 0)
                return;

            firstToken = parsedTokenLine[0];

        }


        private void RemoveConditionalTokens(int index)
        {
            parsedTokenLine.RemoveRange(0, index + 1);
        }

        private bool SimplifyConditionalExpression()
        {
            ExecCondition execCondition = new ExecCondition(conditionalTokens.ToArray());

            return execCondition.OkToExecute();
        } 


        private void SimplifyAllExpressions()
        {
            NO_EXPRESSION = parsedTokenLine.Count - 1;

            while (true)
            {
                int expressionStart = FindExpressionStart();

                if (expressionStart == NO_EXPRESSION)
                    break;

                int expressionEnd = FindExpressionEnd(expressionStart);

                SimplifyExpression(expressionStart, expressionEnd);

                NO_EXPRESSION = parsedTokenLine.Count - 1;
            }
        }


        private int FindExpressionStart()
        {
            int expressionStart;

            for (expressionStart = 0; expressionStart < parsedTokenLine.Count; expressionStart++)
                if (parsedTokenLine[expressionStart].IsOperationToken())
                    break;

            return expressionStart - 1;
        }

        private int FindExpressionEnd(int expressionStart)
        {
            int expressionEnd;

            for(expressionEnd = expressionStart; expressionEnd < parsedTokenLine.Count; expressionEnd++)
                if (!parsedTokenLine[expressionEnd].IsExpressionToken())
                    break;

            return expressionEnd;
        }


        private void SimplifyExpression(int expressionStart, int expressionEnd)
        {
            List<Token> tokensToEvaluate = new List<Token>();

            for (int i = expressionStart; i < expressionEnd; i++)
            {
                tokensToEvaluate.Add(parsedTokenLine[expressionStart]);
                parsedTokenLine.RemoveAt(expressionStart);
            }

            Expression expression = new Expression(tokensToEvaluate.ToArray());
            Token evaluatedExpression = expression.Evaluate();

            parsedTokenLine.Insert(expressionStart, evaluatedExpression);
        }


        private void CheckVariableSetterTokens()
        {
            if (parsedTokenLine.Count <= VAR_VAL_INDEX)
                throw new ILSException("Expected more tokens to set variable");


            Token setterToken = parsedTokenLine[VAR_SET_INDEX];
            Token varTypeToken = parsedTokenLine[VAR_TYPE_INDEX];
            Token valueToken = parsedTokenLine[VAR_VAL_INDEX];

            if (!setterToken.IsTypeOf(TokenType.SETVAR))
                throw new ILSException("Expected set variable token, found: " + setterToken);

            if (!varTypeToken.IsTypeOf(TokenType.DECLARENUM, TokenType.DECLARESTR))
                throw new ILSException("Expected a variable declaration token, found: " + varTypeToken);

            if (!valueToken.IsTypeOf(TokenType.VALUE, TokenType.VARIABLE))
                throw new ILSException("Expected a value argument token, found: " + valueToken);
        }


        private void AddNewVariable()
        {
            Token varTypeToken = parsedTokenLine[VAR_TYPE_INDEX];
            Token valueToken = parsedTokenLine[VAR_VAL_INDEX];

            if (valueToken.IsTypeOf(TokenType.VARIABLE))
            {
                parsedTokenLine.Insert(VAR_VAL_INDEX, new Token(VariableMap.GetVariableValue(valueToken.ToString()), TokenType.VALUE));
                parsedTokenLine.RemoveAt(VAR_VAL_INDEX + 1);
                valueToken = parsedTokenLine[VAR_VAL_INDEX];
            }

            string varName = firstToken.ToString();
            string value = valueToken.ToString();

            if (varTypeToken.IsTypeOf(TokenType.DECLARENUM))
                VariableMap.AddNewVariable(varName, GetNumberValue(value));

            else 
                VariableMap.AddNewVariable(varName, value);
        }


        private double GetNumberValue(string value)
        {
            double.TryParse(value, out double result);

            return result;
        }


        private void RunFunction()
        {
            string functionName = parsedTokenLine[FUNCTION_NAME].ToString();
            functionName = GetCorrectedFunctionName(functionName);

            Token[] functionArguments = GetFunctionArguments();

            FunctionMethods fm = new FunctionMethods();

            fm.GetType().GetMethod(functionName).Invoke(fm, new object[] { functionArguments});
        }


        private Token[] GetFunctionArguments()
        {
            List<Token> functionArguments = new List<Token>();

            for (int i = FUNC_ARGS; i < parsedTokenLine.Count; i++)
                functionArguments.Add(parsedTokenLine[i]);

            return functionArguments.ToArray();
        }


        private string GetCorrectedFunctionName(string functionName)
        {
            functionName = functionName.ToLower();
            functionName = char.ToUpper(functionName[0]) + functionName.Substring(1);
            return functionName;
        }
    }
}