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

        public File(string file)
        {
            FileName = file;
            FileLineTokens = new List<TokenLine>();
        }


        public void LoadInputAndExecute()
        {
            StreamReader sr = new StreamReader(FileName);

            string temp = sr.ReadLine();

            int currentLineNumber = 1;

            while (!FoundBeginFile(temp))
            {
                temp = sr.ReadLine();
                currentLineNumber++;
            }

            while (temp != null)
            {
                FileLineTokens.Add(new TokenLine(temp.Trim()));
                temp = sr.ReadLine();
            }

            sr.Close();
            ParseAndExecute();
        }

        private bool FoundBeginFile(string str)
        {
            if (str == null)
                return true;

            string beginFileConstant = GetBeginFileConstant();
            return str.Contains(beginFileConstant);
        }

        private string GetBeginFileConstant()
        {
            foreach (string s in Constants.ConstantToTokenType.Keys)
                if (TokenType.BEGINFILE == Constants.ConstantToTokenType.GetValueOrDefault(s))
                    return s;

            return null;
        }


        private void ParseAndExecute()
        {
            while (LinePointer.GetCurrentLine() < FileLineTokens.Count)
            {
                FileLineTokens[LinePointer.GetCurrentLine() - 1].ParseLine();
                FileLineTokens[LinePointer.GetCurrentLine() - 1].Interpret();
                LinePointer.Increment();
            }
        }

    }
}
