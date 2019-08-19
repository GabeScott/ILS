using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILS
{
    public class FunctionMethods
    {

        public void Print(Token[] args)
        {
            string output = GetOutput(args);
            Console.Write(output);
        }


        private string GetOutput(Token[] args)
        {
            string output = "";

            foreach (Token t in args)
                if (t.IsTypeOf(TokenType.VARIABLE))
                    output += VariableMap.GetVariableValue(t.ToString());
                else if (t.IsTypeOf(TokenType.VALUE))
                    output += t.ToString();
                else
                    Logger.LogErrorAndQuit("Unexpected token: " + t);

            return CleanString(output);
        }


        private string CleanString(string str)
        {
            string returnstring = "";

            foreach (char c in str)
                if (c != Constants.stringLiteralIdentifier)
                    returnstring += c;

            return returnstring;
        }


        public void Printn(Token[] args)
        {
            string output = GetOutput(args);
            Console.WriteLine(output);
        }


        public void Update(Token[] args)
        {
            Token variableToUpdate = args[0];
            Token updateToValue = args[1];

            TokenType varType = VariableMap.GetVariableType(variableToUpdate.ToString());

            if (updateToValue.IsTypeOf(TokenType.VARIABLE))
                updateToValue = new Token(VariableMap.GetVariableValue(updateToValue.ToString()), TokenType.VALUE);

            if (varType == TokenType.VARIABLENUM)
            {
                if (!double.TryParse(updateToValue.ToString(), out double result))
                    Logger.LogErrorAndQuit("Invalid number value: " + updateToValue);

                VariableMap.UpdateVariable(variableToUpdate.ToString(), result);
            }
            else
               VariableMap.UpdateVariable(variableToUpdate.ToString(), updateToValue.ToString());
        }


        private int GetLineJumpNumber(Token[] functionArguments)
        {
            string lineToJumpTo = functionArguments[0].ToString();


            bool validLineNumber = int.TryParse(lineToJumpTo, out int result);

            if (!validLineNumber)
                result = LineMarkers.GetLineByMarker(lineToJumpTo);

            return result;
        }


        public void Jumpto(Token[] args)
        {
            int linenumber = GetLineJumpNumber(args);
            LinePointer.InsertJump(linenumber);
        }


        public void Jumponce(Token[] args)
        {
            int linenumber = GetLineJumpNumber(args);
            LinePointer.InsertJumpOnce(linenumber);
        }


        public void Return(Token[] args)
        {
            LinePointer.ReturnMostRecent();
        }


        public void Delete(Token[] args)
        {
            if (args.Length > 1)
                throw new ILSException("Unexpected tokens after delete");

            if (!args[0].IsTypeOf(TokenType.VARIABLE))
                throw new ILSException("Expected variable token after delete, found " + args[0].ToString());

            VariableMap.DeleteVariable(args[0].ToString());
        }
    }
}
