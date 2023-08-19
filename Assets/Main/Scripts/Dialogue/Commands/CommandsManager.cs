using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Extensions;
using UnityEngine.Events;

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
        private CommandsDatabase database;

        // Coroutine & process management
        private List<CommandProcess> activeProcesses = new();
        private CommandProcess lastProcess => activeProcesses.Last();

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

        public CoroutineWrapper Execute(string commandName, params string[] args)
        {
            Delegate command = database.GetCommand(commandName);
            // Guard to check if we have a command
            if (command == null)
            {
                return null;
            }
            return StartProcess(commandName, command, args);
        }

        private CoroutineWrapper StartProcess(string commandName, Delegate command, string[] args)
        {
            // StopCurrentProcess();
            // Create new CommandProcess to contain data
            System.Guid guid = new();
            CommandProcess commandProcess = new CommandProcess(guid, commandName, command, null, args, null);
            activeProcesses.Add(commandProcess);

            // Create coroutine
            Coroutine coroutine = StartCoroutine(RunningProcess(commandProcess));

            // Create coroutine wrapper
            commandProcess.runningProcess = new CoroutineWrapper(this, coroutine);

            return commandProcess.runningProcess;
        }

        public void StopCurrentProcess()
        {
            if (lastProcess != null)
            {
                KillProcess(lastProcess);
            }
        }

        public void StopAllProcesses()
        {
            foreach (var process in activeProcesses)
            {
                if (process.runningProcess != null && !process.runningProcess.IsDone)
                {
                    process.runningProcess.Stop();
                }
                process.onTerminateAction?.Invoke();
            }
            activeProcesses.Clear();
        }

        private IEnumerator RunningProcess(CommandProcess process)
        {
            yield return WaitingForProcessCompletion(process.command, process.args);
            KillProcess(process);
        }

        public void KillProcess(CommandProcess process)
        {
            activeProcesses.Remove(process);

            if (process.runningProcess != null && !process.runningProcess.IsDone)
            {
                process.runningProcess.Stop();
            }

            // If there's an action to run after killing the process, do it
            process.onTerminateAction?.Invoke();
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

        public void AddTerminationActionToCurrentProcess(UnityAction action)
        {
            if (lastProcess == null)
            {
                return;
            }

            lastProcess.onTerminateAction = new UnityEvent();
            lastProcess.onTerminateAction.AddListener(action);
        }
    }
}