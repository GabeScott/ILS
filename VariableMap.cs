using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class VariableMap
    {
        private static Dictionary<string, Variable> AllVariables = new Dictionary<string, Variable>();

        public static void AddNewVariable(string name, Variable variable)
        {
            AllVariables.Add(name, variable);
        }

        public static Variable GetVarByName(string name)
        {
            return AllVariables.GetValueOrDefault(name);
        }

        public static bool ContainsVariable(string name)
        {
            return AllVariables.ContainsKey(name);
        }
    }
}
