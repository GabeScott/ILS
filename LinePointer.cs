using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class LinePointer
    {
        private static int pointer = 1;
        private static int nextLine = 2;
        private static int MAX_STACK_SIZE = 100;

        private static Stack<int> returnOrder = new Stack<int>();
        private static List<int> usedReturns = new List<int>();
        private static List<int> usedJumps = new List<int>();


        public static void SetFirstLine(int firstLine)
        {
            pointer = firstLine;
            nextLine = pointer + 1;
        }


        public static int GetCurrentLine()
        {
            return pointer;
        }


        public static void Increment()
        {
            pointer = nextLine;
            nextLine++;
        }


        public static void InsertJump(int lineNum)
        {
            if (!CheckValidJump(lineNum))
                return;

            UpdateJump(lineNum);
        }


        public static void InsertJumpOnce(int lineNum)
        {
            if (!CheckValidJump(lineNum))
                return;

            usedJumps.Add(pointer);

            while (usedJumps.Count > MAX_STACK_SIZE)
                usedJumps.RemoveAt(0);

            UpdateJump(lineNum);
        }


        private static bool CheckValidJump(int line)
        {
            if (pointer == line)
                throw new ILSException("Cannot jump to same line");

            if (usedJumps.Exists(element => element == pointer))
                return false;

            return true;
        }


        private static void UpdateJump(int line)
        {
            if (returnOrder.Count > MAX_STACK_SIZE)
                returnOrder.Clear();

            returnOrder.Push(pointer + 1);

            nextLine = line;
        }


        public static void ReturnMostRecent()
        {
            usedReturns.Add(pointer);

            nextLine = returnOrder.Pop();

        }
    }
}
