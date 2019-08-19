using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{

    public enum FunctionName
        {
            PRINT,
            PRINTN,
            UPDATE, 
            JUMPTO,
            JUMPONCE,
            RETURN
        }


    static class Functions
    {
        public static void RunFunction(string function, Token[] functionArguments)
        {
            string output = GetOutput(functionArguments);

            FunctionName result;
            Enum.TryParse(function, out result);

            if (result == FunctionName.PRINT)
                Console.Write(output);

            else if (result == FunctionName.PRINTN)
                Console.WriteLine(output);

            else if (result == FunctionName.UPDATE)
                UpdateVariable(functionArguments);

            else if (result == FunctionName.JUMPTO)
                LinePointer.InsertJump(GetLineJumpNumber(functionArguments));

            else if (result == FunctionName.JUMPONCE)
                LinePointer.InsertJumpOnce(GetLineJumpNumber(functionArguments));

            else if (result == FunctionName.RETURN)
                ReturnPointer(functionArguments);

        }

        private static string GetOutput(Token[] functionArguments)
        {
            string output = "";

            foreach (Token t in functionArguments)
                if (t.IsTypeOf(TokenType.VARIABLE))
                    output += VariableMap.GetVariableValue(t.ToString());
                else if (t.IsTypeOf(TokenType.VALUE))
                    output += t.ToString();
                else
                    throw new ILSException("Unexpected token: " + t);

            return CleanStringLiteral(output);
        }

        private static string CleanStringLiteral(string stringLiteral)
        {
            string cleanedString = "";

            foreach (char c in stringLiteral)
                if (c != Constants.stringLiteralIdentifier)
                    cleanedString += c;

            return cleanedString;
        }

        private static void UpdateVariable(Token[] functionArguments)
        {
            string variableName = functionArguments[0].ToString();
            string variableVal = functionArguments[1].ToString();

            VariableMap.UpdateVariable(variableName, variableVal);
        }


        private static int GetLineJumpNumber(Token[] functionArguments)
        {
            string lineToJumpTo = functionArguments[0].ToString();


            bool validLineNumber = int.TryParse(lineToJumpTo, out int result);

            if (!validLineNumber)
                throw new ILSException("Invalid line jump request: " + lineToJumpTo);

            return result;
        }

        private static void ReturnPointer(Token[] functionArguments)
        {
            if (functionArguments.Length > 0)
                throw new ILSException("Return does not take arguments");

            LinePointer.ReturnMostRecent();
        }

    }
}
