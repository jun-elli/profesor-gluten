using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Dialogue
{
    public class DL_Commands
    {
        public List<Command> commands;
        private const char CommandSplitterID = ',';
        private const char ArgumentsContainerID = '(';
        private const string WaitCommandID = "[wait]";
        public struct Command
        {
            public string name;
            public string[] arguments;
            public bool isWaiting;
        }

        public DL_Commands(string rawCommands)
        {
            commands = RipCommands(rawCommands);
        }

        private List<Command> RipCommands(string rawCommands)
        {
            string[] data = rawCommands.Split(CommandSplitterID, System.StringSplitOptions.RemoveEmptyEntries);
            List<Command> result = new List<Command>();

            foreach (string line in data)
            {
                Command command = new Command();
                int index = line.IndexOf(ArgumentsContainerID);

                command.name = line.Substring(0, index).Trim();
                if (command.name.ToLower().StartsWith(WaitCommandID))
                {
                    command.name = command.name.Substring(WaitCommandID.Length);
                    command.isWaiting = true;
                }
                else
                {
                    command.isWaiting = false;
                }

                string args = line.Substring(index + 1, line.Length - index - 2);
                command.arguments = GetArguments(args);
                result.Add(command);
            }
            return result;
        }

        // Get arguments from string
        private string[] GetArguments(string args)
        {
            List<string> argsList = new List<string>();
            StringBuilder currentArg = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (!inQuotes && args[i] == ' ')
                {
                    argsList.Add(currentArg.ToString());
                    currentArg.Clear();
                    continue;
                }

                currentArg.Append(args[i]);
            }

            if (currentArg.Length > 0)
            {
                argsList.Add(currentArg.ToString());
            }
            return argsList.ToArray();
        }
    }
}