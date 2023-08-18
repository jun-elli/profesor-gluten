using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper for the command database, classifing text and flags from the paramters string[]
/// </summary>

namespace Dialogue.Commands
{
    public class CommandParameters
    {
        private const char ParameterIdentifier = '?';
        private Dictionary<string, string> parameters = new();

        public CommandParameters(string[] parameterArray)
        {
            for (int i = 0; i < parameterArray.Length; i++)
            {
                if (parameterArray[i].StartsWith(ParameterIdentifier))
                {
                    string pName = parameterArray[i];
                    string pValue = "";

                    // If there are still parameters and the next one does not start with Identifier, we have a value
                    if (i + 1 < parameterArray.Length && !parameterArray[i + 1].StartsWith(ParameterIdentifier))
                    {
                        pValue = parameterArray[i + 1];
                        i++;
                    }
                    parameters.Add(pName, pValue);
                }
            }
        }

        // We might have only one name for an identifier (?i) or multiple (?i, ?immediate)
        public bool TryGetValue<T>(string parameterName, out T value, T defaultValue = default) => TryGetValue(new string[] { parameterName }, out value, defaultValue);
        public bool TryGetValue<T>(string[] parameterNames, out T value, T defaultValue = default)
        {
            foreach (string pName in parameterNames)
            {
                if (parameters.TryGetValue(pName, out string pValue))
                {
                    if (TryCastParameter(pValue, out value))
                    {
                        return true;
                    }
                }
            }
            value = defaultValue;
            return false;
        }

        private bool TryCastParameter<T>(string pValue, out T value)
        {
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(pValue, out bool boolValue))
                {
                    value = (T)(object)boolValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(pValue, out int intValue))
                {
                    value = (T)(object)intValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(pValue, out float floatValue))
                {
                    value = (T)(object)floatValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                value = (T)(object)pValue;
                return true;
            }

            value = default;
            return false;
        }
    }
}
