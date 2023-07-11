using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// List of commands
    /// </summary>
    public class CommandsDatabase
    {
        private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

        public bool HasCommand(string commandName)
        {
            return false;
        }
    }
}