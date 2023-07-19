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
        private TextArchitect _architect = null;

        // Has user prompt to advance to next line
        private bool hasUserPrompt = false;

        public ConversationManager(TextArchitect architect)
        {
            this._architect = architect;
            dialogueSystem.onUserPromptAdvance += OnUserPromptAdvance;
        }

        private void OnUserPromptAdvance()
        {
            hasUserPrompt = true;
        }

        // Make sure previous conversation is over and then start new one
        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
            return process;
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

                if (line.hasDialogue)
                {
                    // Wait for user input
                    yield return WaitForUserInput();
                }
            }
        }
        IEnumerator RunDialogue(DialogueLine line)
        {
            // Show name tag or not
            if (line.hasSpeaker)
            {
                dialogueSystem.ShowSpeakerName(line.speakerData.displayName);
            }
            else
            {
                dialogueSystem.HideSpeakerName();
            }

            // Build dialogue line
            yield return BuildLineSegments(line.dialogueData);
        }
        IEnumerator RunCommands(DialogueLine line)
        {
            List<DL_Commands.Command> commands = line.commandsData.commands;

            foreach (DL_Commands.Command command in commands)
            {
                if (command.isWaiting)
                {
                    yield return CommandsManager.Instance.Execute(command.name, command.arguments);
                }
                else
                {
                    CommandsManager.Instance.Execute(command.name, command.arguments);
                }
            }
            yield return null;
        }

        // Build all segments of a dialogue line
        IEnumerator BuildLineSegments(DL_Text line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_Text.Segment segment = line.segments[i];

                yield return WaitForSegmentSignalToBeTriggered(segment);
                yield return BuildDialogueText(segment.text, segment.shouldAppend);
            }
        }

        IEnumerator WaitForSegmentSignalToBeTriggered(DL_Text.Segment segment)
        {
            switch (segment.signal)
            {
                case DL_Text.Segment.StartSignal.C:
                case DL_Text.Segment.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_Text.Segment.StartSignal.WC:
                case DL_Text.Segment.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }
        // Send text to architect
        IEnumerator BuildDialogueText(string text, bool append = false)
        {
            // Build text in architect
            if (!append)
            {
                _architect.Build(text);
            }
            else
            {
                _architect.Append(text);
            }

            // Wait for text to complete
            while (_architect.isBuilding)
            {
                if (!_architect.hurryUp)
                {
                    _architect.hurryUp = true;
                }
                else
                {
                    _architect.ForceComplete();
                }
                hasUserPrompt = false;
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!hasUserPrompt)
            {
                yield return null;
            }
            hasUserPrompt = false;
        }
    }
}