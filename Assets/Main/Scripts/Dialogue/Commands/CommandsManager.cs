using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace Dialogue.Commands
{
    /// <summary>
    /// Class in charge of looking for commands in DB and executing them.
    /// Will recieve requests.
    /// </summary>
    public class CommandsManager : MonoBehaviour
    {
        // We make it a singleton
        public static CommandsManager Instance { get; private set; }
        private Coroutine currentProcess = null;
        private bool isProcessRunning => currentProcess != null;
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

        public Coroutine Execute(string commandName, params string[] args)
        {
            Delegate command = database.GetCommand(commandName);
            // Guard to check if we have a command
            if (command == null)
            {
                return null;
            }
            return StartProcess(commandName, command, args);
        }

        private Coroutine StartProcess(string commandName, Delegate command, string[] args)
        {
            StopCurrentProcess();

            currentProcess = StartCoroutine(RunningProcess(command, args));
            return currentProcess;
        }

        private void StopCurrentProcess()
        {
            if (currentProcess != null)
            {
                StopCoroutine(currentProcess);
            }
            currentProcess = null;
        }
        private IEnumerator RunningProcess(Delegate command, string[] args)
        {
            yield return WaitingForProcessCompletion(command, args);
            currentProcess = null;
        }

        private IEnumerator WaitingForProcessCompletion(Delegate command, string[] args)
        {
            if (command is Action)
            {
                command.DynamicInvoke();
            }
            else if (command is Action<string>)
            {
                command.DynamicInvoke(args[0]);
            }
            else if (command is Action<string[]>)
            {
                command.DynamicInvoke((object)args);
            }
            else if (command is Func<IEnumerator> func0)
            {   // We cast it as Func type and run the command
                yield return func0();
            }
            else if (command is Func<string, IEnumerator> func1)
            {
                yield return func1(args[0]);
            }
            else if (command is Func<string[], IEnumerator> func2)
            {
                yield return func2(args);
            }
        }
    }
}