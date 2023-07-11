using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace Dialogue
{
    /// <summary>
    /// Class in charge of looking for commands in DB and executing them.
    /// Will recieve requests.
    /// </summary>
    public class CommandsManager : MonoBehaviour
    {
        // We make it a singleton
        public static CommandsManager Instance { get; private set; }
        private CommandsDatabase database;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                database = new CommandsDatabase();

                // Get all types of extension commands //
                // We reach out to all the code of our assembly
                Assembly assembly = Assembly.GetExecutingAssembly();
                // We look for all the types that are a subclass of CMD_DatabaseExtension
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

                // We execute the Extend method of each of them to assign all of their commands
                // to the database
                foreach (Type extension in extensionTypes)
                {
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { database });
                }
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public void Execute(string commandName)
        {
            Delegate command = database.GetCommand(commandName);
            if (command != null)
            {
                command.DynamicInvoke();
            }
        }
    }
}