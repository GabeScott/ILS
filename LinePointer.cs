using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class LinePointer
    {
        private static int pointer = 1;

        private static Stack<int> returnOrder = new Stack<int>();
        private static List<int> usedReturns = new List<int>();
        private static List<int> usedJumps = new List<int>();

        private static bool jumpInserted = false;
        private static bool returned = false;


        public static int GetCurrentLine()
        {
            return pointer;
        }

        public static void Increment()
        {
            if (!jumpInserted && !returned)
                pointer++;

            jumpInserted = false;
            returned = false;
        }

        public static void InsertJump(int lineNum)
        {
            if (pointer == lineNum)
                throw new InvalidJumpException("Cannot jump to same line");

            if (usedJumps.Exists(element => element == pointer))
                return;

            returnOrder.Push(pointer + 1);
            pointer = lineNum;
            jumpInserted = true;
        }

        public static void InsertJumpOnce(int lineNum)
        {
            usedJumps.Add(pointer);
            InsertJump(lineNum);
        }

        public static void ReturnMostRecent()
        {
            if (usedReturns.Exists(element => element == pointer))
                return;

            if (returnOrder.Count > 0)
            {
                usedReturns.Add(pointer);
                pointer = returnOrder.Pop();
                returned = true;
                
            }
        }





    }
}
