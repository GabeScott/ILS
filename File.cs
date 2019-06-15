using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILS
{
    class File
    {
        private List<TokenLine> FileLineTokens;
        private string FileName;
        private int currentIndex = 0;

        public File(string file)
        {
            FileName = file;
        }

        public void ReadInput()
        {
            FileLineTokens = new List<TokenLine>();


            StreamReader sr = new StreamReader(FileName);

            string temp = sr.ReadLine();

            while (temp != null)
            {
                FileLineTokens.Add(new TokenLine(temp.Trim()));
                temp = sr.ReadLine();
            }

            sr.Close();


        }


        public void Interpret()
        {
            try
            {
                TryStart();
                TryEnd();
            }catch(Exception e)
            {
                Logger.LogErrorAndQuit(e.Message);
            }



            while(currentIndex < FileLineTokens.Count-1)
            {
                Token curToken = FileLineTokens[currentIndex].GetNextToken();
                TokenType type = curToken.GetType();

                if (curToken.ToString() != "")
                {
                    if (type == TokenType.VARIABLE)
                        try
                        {
                            AddNewVariable(curToken.ToString());
                        }
                        catch (Exception e)
                        {
                            Logger.LogErrorAndQuit(e.Message);
                        }
                    else if (type == TokenType.FUNCTION)
                        try
                        {
                            RunFunction(curToken.ToString());
                        }
                        catch (Exception e)
                        {
                            Logger.LogErrorAndQuit(e.Message);
                        }
                }

                currentIndex++;
            }


          

        }

        private void TryStart()
        {
            if (GetFirstToken().GetType() != TokenType.BEGINFILE)
                throw new Exception("Invalid BeginLine Token");
            currentIndex = 1;

        }

        private void TryEnd()
        {
            if (GetLastToken().GetType() != TokenType.ENDFILE)
                throw new Exception("Invalid End Line Token");
        }

        private Token GetFirstToken()
        {
            return FileLineTokens[0].GetTokenAt(0);
        }

        private Token GetLastToken()
        {
            int len = FileLineTokens.Count;
            return FileLineTokens[len-1].GetTokenAt(0);
        }

        private void RunFunction(string function)
        {
            List<Token> paramTokens = new List<Token>();

            while (FileLineTokens[currentIndex].HasNext())
                paramTokens.Add(FileLineTokens[currentIndex].GetNextToken());

            Functions.RunFunction(function, paramTokens.ToArray());


            var endlinetoken = FileLineTokens[currentIndex].GetLastToken();

            try
            {
                TryLastTokenInLine(endlinetoken);
            }
            catch
            {
                throw;
            }


        }

        private void AddNewVariable(string name)
        {

            Token setvartoken, typetoken, valtoken, endlinetoken;

            setvartoken = FileLineTokens[currentIndex].GetNextToken();
            typetoken = FileLineTokens[currentIndex].GetNextToken();
            valtoken = FileLineTokens[currentIndex].GetNextToken();

            if (FileLineTokens[currentIndex].HasNext())
                throw new ILSTooManyTokensException("Unexpected Token: " + FileLineTokens[currentIndex].GetNextToken());

            endlinetoken = FileLineTokens[currentIndex].GetLastToken();

            if (setvartoken == null || typetoken == null || valtoken == null || endlinetoken == null)
                throw new ILSTooFewTokensException("Expected more tokens");

            if (setvartoken.GetType() == TokenType.SETVAR && typetoken.GetType() == TokenType.VARTYPENUM && valtoken.GetType() == TokenType.VARIABLE)
            {
                double result;
                if (Double.TryParse(valtoken.ToString(), out result))
                    VariableMap.AddNewVariable(name, new ILSVariableNum(name, result));
                else
                    throw new Exception("Invalid number value");
            }

            else if (setvartoken.GetType() == TokenType.SETVAR && typetoken.GetType() == TokenType.VARTYPESTR && valtoken.GetType() == TokenType.VARIABLE)
            {
                VariableMap.AddNewVariable(name, new ILSVariableStr(name, valtoken.ToString()));
            }

            try
            {
                TryLastTokenInLine(endlinetoken);
            }catch(Exception e)
            {
                throw;
            }

        }

        private void TryLastTokenInLine(Token lastToken)
        {
            if (lastToken.GetType() != TokenType.ENDLINE)
                throw new Exception("Expected " + Constants.ENDLINE + " at the end of the line");
        }
    }
}
