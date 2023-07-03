using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.Instance;
        private Coroutine process = null;
        public bool isRunning => process != null;

        public void StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        private void StopConversation()
        {
            if (!isRunning)
            {
                return;
            }
            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                // if blank line, skip
                if (string.IsNullOrWhiteSpace(conversation[i]))
                {
                    continue;
                }
                DialogueLine line = DialogueParser.Parse(conversation[i]);

                // show dialogue
                if (line.hasDialogue)
                {
                    // Call corutine while inside corutine
                    yield return RunDialogue(line);
                }
                // run commands
                if (line.hasCommands)
                {
                    yield return RunCommands(line);
                }
            }
        }
        IEnumerator RunDialogue(DialogueLine line)
        {
            if (line.hasSpeaker)
            {

            }
            yield return null;
        }
        IEnumerator RunCommands(DialogueLine line)
        {
            yield return null;
        }
    }
}