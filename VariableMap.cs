using System;
using System.Collections.Generic;
using System.Text;

namespace ILS
{
    static class VariableMap
    {
        private static Dictionary<string, Variable> AllVariables = new Dictionary<string, Variable>();

        public static void AddNewVariable(string name, double value)
        {
            if (AllVariables.ContainsKey(name))
                throw new InvalidVariableNameException("Variable " + name + " already exists");
            else
                AllVariables.Add(name, new NumberVariable(name, value));
        }

        public static void AddNewVariable(string name, string value)
        {
            if (AllVariables.ContainsKey(name))
                throw new InvalidVariableNameException("Variable " + name + " already exists");
            else
                AllVariables.Add(name, new StringVariable(name, value));
        }


        public static void UpdateVariable(string variableName, string variableVal)
        {
            TokenType varType = GetVariableType(variableName);

            if(varType == TokenType.VARIABLESTR)
                AllVariables[variableName] = new StringVariable(variableName, variableVal);

            else
            {
                if (!TokenRules.IsValidNumberValue(variableVal))
                    throw new InvalidNumberValueException("Invalid number to update variable to: " + variableVal);

                double.TryParse(variableVal, out double result);
                AllVariables[variableName] = new NumberVariable(variableName, result);

            }
        }


        public static string GetVariableValue(string variableName)
        {
            Variable v = GetVarByName(variableName);

            return v.GetValAsString();
        }


        public static TokenType GetVariableType(string variableName)
        {
            Variable v = GetVarByName(variableName);

            return v.GetType();
        }


        private static Variable GetVarByName(string variableName)
        {
            CheckIfVariableExists(variableName);

            return AllVariables.GetValueOrDefault(variableName);
        }


        private static void CheckIfVariableExists(string varName)
        {
            Variable v = AllVariables.GetValueOrDefault(varName);

            if (v == null)
                throw new InvalidVariableNameException("Variable does not exist: " + varName);
        }
    }
}
