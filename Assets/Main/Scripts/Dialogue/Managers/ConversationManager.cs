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
            // Show name tag or not
            if (line.hasSpeaker)
            {
                dialogueSystem.ShowSpeakerName(line.speaker.displayName);
            }
            else
            {
                dialogueSystem.HideSpeakerName();
            }

            // Build dialogue line
            yield return BuildLineSegments(line.dialogue);

            // Wait for user input
            yield return WaitForUserInput();
        }
        IEnumerator RunCommands(DialogueLine line)
        {
            Debug.Log(line.commands);
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