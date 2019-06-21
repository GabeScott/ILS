using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class VariableMap
    {
        private static Dictionary<string, Variable> AllVariables = new Dictionary<string, Variable>();

        public static void AddNewVariable(string variableName, Variable variable)
        {
            AllVariables.Add(variableName, variable);
        }

        public static Variable GetVarByName(string variableName)
        {
            return AllVariables.GetValueOrDefault(variableName);
        }

        public static bool ContainsVariable(string variableName)
        {
            return AllVariables.ContainsKey(variableName);
        }
    }
}
