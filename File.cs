using System.Collections.Generic;
using System.IO;

namespace ILS
{
    class File
    {
        private List<TokenLine> fileLineTokens;
        private readonly string fileName;
        private int beginFileTokenLineNumber = 1;

        public File(string file)
        {
            fileName = file;
            fileLineTokens = new List<TokenLine>();
        }


        public void LoadAllLinesAndExecute()
        {
            using (StreamReader sr = new StreamReader(fileName))
            {

                string temp = sr.ReadLine();
                LinePointer.SetFirstLine(beginFileTokenLineNumber);


                while (!FoundBeginFile(temp))
                {
                    fileLineTokens.Add(new TokenLine(temp.Trim()));
                    LinePointer.Increment();
                    temp = sr.ReadLine();
                    beginFileTokenLineNumber++;
                }


                while (temp != null)
                {
                    fileLineTokens.Add(new TokenLine(temp.Trim()));
                    LinePointer.Increment();
                    temp = sr.ReadLine();
                }

                LinePointer.SetFirstLine(beginFileTokenLineNumber);

            }

            LoadLineMarkers();
            ParseAndExecute();
        }

        private bool FoundBeginFile(string str)
        {
            if (str == null)
                return true;

            return str.Contains(Constants.beginFileToken);
        }


        private void LoadLineMarkers()
        {
            for (int i = 1; i < fileLineTokens.Count; i++)
                fileLineTokens[i - 1].CheckForLineMarker(i);
        }


        private void ParseAndExecute()
        {
            while (LinePointer.GetCurrentLine() < fileLineTokens.Count)
            {
                int line = LinePointer.GetCurrentLine();
                fileLineTokens[line - 1].ParseAndExecute();
                LinePointer.Increment();
            }
        }

    }
}
