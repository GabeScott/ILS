using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILS
{

    public class StateTransitioner
    {
        private int currentState = 0;


        public void UpdateState(char nextChar)
        {
            StateData.AllStates.TryGetValue(currentState, out Dictionary<string, int> stateDict);
            int nextState = StateData.INVALID_STATE;

            foreach (string key in stateDict.Keys)
                if (key.Contains(nextChar.ToString()))
                {
                    stateDict.TryGetValue(key, out nextState);
                    break;
                }

            if (nextState == StateData.INVALID_STATE && currentState == StateData.STRING_LIT_STATE)
                currentState = StateData.STRING_LIT_STATE;

            else
                currentState = nextState;
        }



        public TokenType GetTokenTypeFromState(int state)
        {
            if (StateData.stateToTokenType.TryGetValue(state, out TokenType tt))
                return tt;

            return TokenType.UNKNOWN;

        }


        public void ResetState()
        {
            currentState = StateData.INITIAL_STATE;
        }


        public bool IsMultiCharacterToken()
        {
            return currentState == StateData.VARIABLE_LENGTH_STATE;
        }


        public bool IsSingleCharacterToken()
        {
            return currentState == StateData.SINGLE_CHAR_STATE;
        }


        public bool IsInvalidState()
        {
            return currentState == StateData.INVALID_STATE;
        }


        public TokenType GetCurrentTokenType()
        {
            if (StateData.stateToTokenType.TryGetValue(currentState, out TokenType tt))
                return tt;

            return TokenType.UNKNOWN;
        }


        protected static class StateData
        {

            public static readonly int STRING_LIT_STATE = 11;
            public static readonly int VARIABLE_LENGTH_STATE = 13;
            public static readonly int SINGLE_CHAR_STATE = 14;
            public static readonly int INVALID_STATE = -1;
            public static readonly int INITIAL_STATE = 0;



            private static readonly string allLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private static readonly string allNumbers = "0123456789";
            private static readonly string highExp = "*/";
            private static readonly string lowExp = "-+";
            private static readonly string condStart = "?";
            private static readonly string lineMarkerStart = "@";
            private static readonly string condExp = "><=#";
            private static readonly string stringLiteral = "'";
            private static readonly string decimalPointString = ".";
            private static readonly string emptySpace = " ";
            private static readonly string extraVariableChars = "_!";
            private static readonly string validAfterTokens = condExp + highExp + lowExp + emptySpace + condStart;


            public static Dictionary<int, TokenType> stateToTokenType = new Dictionary<int, TokenType>()
            {
                { 0, TokenType.SINGLE_CHAR },
                { 1, TokenType.KEYWORD },
                { 2, TokenType.KEYWORD },
                { 3, TokenType.VALUE },
                { 4, TokenType.VALUE },
                { 6, TokenType.LINE_MARKER },
                { 12, TokenType.VALUE },
            };


            public static Dictionary<int, Dictionary<string, int>> AllStates = new Dictionary<int, Dictionary<string, int>>()
            {
                {0, new Dictionary<string, int>()
                    {
                        { emptySpace, 14 },
                        { allLetters, 1 },
                        { allNumbers, 3 },
                        { lineMarkerStart, 5 },
                        { condStart, 14 },
                        { condExp, 14 },
                        { lowExp, 14 },
                        { highExp, 14 },
                        { stringLiteral, 11 }
                    }
                },
                {1, new Dictionary<string, int>()
                    {
                        { allNumbers + allLetters + extraVariableChars, 2 },
                        { validAfterTokens, 13 }
                    }
                },
                {2, new Dictionary<string, int>()
                    {
                        { allNumbers + allLetters + extraVariableChars, 2 },
                        { validAfterTokens, 13 }
                    }
                },
                {3, new Dictionary<string, int>()
                    {
                        { allNumbers, 3 },
                        { decimalPointString, 4 },
                        { validAfterTokens, 13 }
                    }
                },
                {4, new Dictionary<string, int>()
                    {
                        { allNumbers, 4 },
                        { validAfterTokens, 13 }
                    }
                },
                {5, new Dictionary<string, int>()
                    {
                        { allLetters, 6 }
                    }
                },
                {6, new Dictionary<string, int>()
                    {
                        { allLetters, 6 }, { emptySpace, 13 }
                    }
                },
                {11, new Dictionary<string, int>()
                    {
                        { stringLiteral, 12 }
                    }
                },
                {12, new Dictionary<string, int>()
                    {
                        { lowExp + emptySpace, 13 }
                    }
                },
                {13, new Dictionary<string, int>() },
                {14, new Dictionary<string, int>() }

            };
        }
    }
}
