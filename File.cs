using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILS
{
    class File
    {
        private List<TokenLine> FileLineTokens;
        private readonly string FileName;
        private int currentIndex = 0;

        private readonly int VAR_NAME_INDEX = 0;
        private readonly int SET_VAR_INDEX = 1;
        private readonly int VAR_TYPE_INDEX = 2;
        private readonly int VAL_INDEX = 3;

        private readonly int FUNCTION_NAME_INDEX = 0;

        public File(string file)
        {
            FileName = file;
        }

        public void LoadInputAndExecute()
        {
            ReadInput();

            while (currentIndex < FileLineTokens.Count)//TODO Make it loop until the SLM! is found, as there could be two SLM! 
            {
                TryLastTokenInLine();

                Token firstTokenInLine = FileLineTokens[currentIndex].GetNextToken();
                TokenType type = firstTokenInLine.Type;

                if (type == TokenType.VARIABLE)
                {
                    CheckVariableSetterTokens();
                    AddNewVariable();
                }
                else if (type == TokenType.FUNCTION)
                    RunFunction();



                currentIndex++;
            }
        }

        private void ReadInput()
        {
            FileLineTokens = new List<TokenLine>();

            StreamReader sr = new StreamReader(FileName);

            string temp = sr.ReadLine();

            int currentLineNumber = 1;

            bool foundEndOfFile = false;

            while (temp != null && !foundEndOfFile)
            {
                TokenLine tl = new TokenLine(temp.Trim(), currentLineNumber++);

                foundEndOfFile = tl.ContainsEndOfFile();
                FileLineTokens.Add(tl);

                temp = sr.ReadLine();
            }
            
            sr.Close();

            TryFileBeginToken();
            //TryFileEndToken();
            RemoveEmptyTokenLines();

        }

        private void TryFileBeginToken()
        {
            Token firstToken = FileLineTokens[0].GetTokenAt(0);

            if (!firstToken.IsTypeOf(TokenType.BEGINFILE))
                throw new InvalidBeginFileException("Invalid Begin File Token: " + firstToken.ToString());
            currentIndex = 1;

        }

        private void TryFileEndToken()
        {
            Token lastToken = FileLineTokens[FileLineTokens.Count-1].GetTokenAt(0);

            if (!lastToken.IsTypeOf(TokenType.ENDFILE))
                throw new InvalidEndFileException("Invalid End File Token: " + lastToken.ToString());
        }



        private void RemoveEmptyTokenLines()
        {
            for (int i = 0; i < FileLineTokens.Count; i++)
            {
                TokenLine tl = FileLineTokens[i];
                if (tl.IsEmpty())
                {
                    FileLineTokens.RemoveAt(i);
                    i--;
                }
            }
        }

        private void CheckVariableSetterTokens()
        {
            Token setvartoken, typetoken;

            setvartoken = FileLineTokens[currentIndex].GetTokenAt(SET_VAR_INDEX);
            typetoken = FileLineTokens[currentIndex].GetTokenAt(VAR_TYPE_INDEX);


            if (setvartoken == null || typetoken == null)
                throw new TooFewTokensException("Expected more tokens on line " + FileLineTokens[currentIndex].LineNumberInFile);

            if (!setvartoken.IsTypeOf(TokenType.SETVAR))
                throw new InvalidTokenException("Invalid setvar token one line " + FileLineTokens[currentIndex].LineNumberInFile);

            if(!typetoken.IsTypeOf(TokenType.DECLARENUM) && !typetoken.IsTypeOf(TokenType.DECLARESTR))
                throw new InvalidTokenException("Invalid variable type token one line " + FileLineTokens[currentIndex].LineNumberInFile);
        }

        private void AddNewVariable()
        {

            Token nametoken, typetoken, valtoken;

            nametoken = FileLineTokens[currentIndex].GetTokenAt(VAR_NAME_INDEX);
            typetoken = FileLineTokens[currentIndex].GetTokenAt(VAR_TYPE_INDEX);
            valtoken = FileLineTokens[currentIndex].GetTokenAt(VAL_INDEX);

            string name = nametoken.ToString();
            string varValue = valtoken.ToString();


            if (typetoken.IsTypeOf(TokenType.DECLARENUM))
            {
                if (double.TryParse(varValue, out double result))
                    VariableMap.AddNewVariable(name, new NumberVariable(name, result));
                else
                    throw new InvalidNumberValueException("Invalid number value");
            }
            else if (typetoken.IsTypeOf(TokenType.DECLARESTR))
                VariableMap.AddNewVariable(name, new StringVariable(name, varValue));


        }

        private void TryLastTokenInLine()
        {
            Token lastToken = FileLineTokens[currentIndex].GetLastToken();

            if (lastToken.Type != TokenType.ENDLINE && lastToken.Type != TokenType.ENDFILE)
                throw new InvalidEndLineException("Expected " + Constants.GetConstantByTokenType(TokenType.ENDLINE) + " at the end of the line");
        }

        private void RunFunction()
        {
            string functionName = FileLineTokens[currentIndex].GetTokenAt(FUNCTION_NAME_INDEX).ToString();

            List<Token> functionArguments = new List<Token>();

            Token currToken = null;
            TokenType tt = TokenType.VALUE;


            while (FileLineTokens[currentIndex].HasNext())
            {
                currToken = FileLineTokens[currentIndex].GetNextToken();
                tt = currToken.Type;

                if(tt == TokenType.VARIABLE || tt == TokenType.VALUE)
                    functionArguments.Add(currToken);
            }

            Functions.RunFunction(functionName, functionArguments.ToArray());


        }

    }
}
