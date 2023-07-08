using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dialogue
{
    // Will convert textasset lines to DialogueLine
    public class DialogueParser
    {
        // \w - word character
        // * - any number of times
        // [^XXX] - don't match any of the XXX characters
        // \s - white space
        // \( - parenthesis
        private const string CommandsRegexPattern = "\\w*[^\\s]\\(";

        public static DialogueLine Parse(string rawLine)
        {
            // Debug.Log($"Parsing line: {rawLine}");
            Debug.Log("Inside ConversationManager: Parse()");
            (string speaker, string dialogue, string commands) = RipContent(rawLine);
            return new DialogueLine(speaker, dialogue, commands);
        }

        private static (string, string, string) RipContent(string rawLine)
        {
            string speaker = "", dialogue = "", commands = "";

            // Find dialogue
            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for (int i = 0; i < rawLine.Length; i++)
            {
                if (rawLine[i] == '\\')
                {
                    isEscaped = !isEscaped;
                }
                else if (rawLine[i] == '"' && !isEscaped)
                {
                    if (dialogueStart == -1)
                    {
                        dialogueStart = i;
                    }
                    else if (dialogueEnd == -1)
                    {
                        dialogueEnd = i;
                        break;
                    }
                }
                else
                {
                    isEscaped = false;
                }
            }

            // FIND COMMAND AND SPEAKER

            Regex commandRegex = new Regex(CommandsRegexPattern);
            Match match = commandRegex.Match(rawLine);

            // Find command start index if any
            int commandStart = -1;
            if (match.Success)
            {
                commandStart = match.Index;

                // if we have not found dialogue, return only command as it is all the line
                if (dialogueStart == -1 && dialogueEnd == -1)
                {
                    return ("", "", rawLine.Trim());
                }
            }
            // check each case

            // if we have indexes for dialogue, 
            // and either there is no command found or the index for the command is after the dialogue
            // we have (maybe) speaker, dialogue and (maybe) command
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                speaker = rawLine.Substring(0, dialogueStart).Trim(); // might be 0 to 0, will stary empty
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1)
                    .Replace("\\\"", "\"");

                if (commandStart != -1)
                {
                    commands = rawLine.Substring(commandStart).Trim();
                }
            } // if we have a command with arguments
            else if (commandStart != -1 && dialogueStart > commandStart)
            {
                commands = rawLine;
            }
            else
            {
                speaker = rawLine;
            }
            // Debug.Log($"Speaker: {speaker}, \nDialogue: {dialogue}, \nCommand: {commands}");
            return (speaker, dialogue, commands);
        }
    }
}
