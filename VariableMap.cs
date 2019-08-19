using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class VariableMap
    {
        private static Dictionary<string, Variable> AllVariables = new Dictionary<string, Variable>();

        public static void AddNewVariable(string newVarName, double newVarValue)
        {
            if (AllVariables.ContainsKey(newVarName))
                throw new ILSException("Variable " + newVarName + " already exists");
            else
                AllVariables.Add(newVarName, new NumberVariable(newVarName, newVarValue));
        }


        public static void AddNewVariable(string newVarName, string newVarValue)
        {
            if (AllVariables.ContainsKey(newVarName))
                throw new ILSException("Variable " + newVarName + " already exists");
            else
                AllVariables.Add(newVarName, new StringVariable(newVarName, newVarValue));
        }


        public static void DeleteVariable(string varName)
        {
            if (!AllVariables.ContainsKey(varName))
                throw new ILSException("Variable " + varName + " does not exist and cannot be deleted");
            else
                AllVariables.Remove(varName);
        }


        public static void UpdateVariable(string existingVariable, double newValue)
        {
            AllVariables[existingVariable] = new NumberVariable(existingVariable, newValue);
        }


        public static void UpdateVariable(string existingVariable, string newValue)
        {
            AllVariables[existingVariable] = new StringVariable(existingVariable, newValue);
        }


        public static string GetVariableValue(string varToRetrive)
        {
            Variable v = GetVarByName(varToRetrive);

            return v.GetValAsString();
        }


        public static TokenType GetVariableType(string varToRetrive)
        {
            Variable v = GetVarByName(varToRetrive);

            return v.GetType();
        }


        private static Variable GetVarByName(string varToRetrive)
        {
            if (!AllVariables.TryGetValue(varToRetrive, out Variable v))
                throw new ILSException("Invalid variable name: " + varToRetrive);

            return v;
        }
    }
}
