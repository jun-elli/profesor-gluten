using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    // Will convert textasset lines to DialogueLine
    public class DialogueParser
    {
        public static DialogueLine Parse(string rawLine)
        {
            Debug.Log($"Parsing line: {rawLine}");

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
            dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1);

            // Find command


            return (speaker, dialogue, commands);
        }
    }
}
