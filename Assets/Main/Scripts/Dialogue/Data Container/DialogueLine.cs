using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogueLine
    {
        public DL_Speaker speakerData;
        public DL_Text dialogueData;
        public DL_Commands commandsData;

        public bool hasSpeaker => speakerData != null;
        public bool hasDialogue => dialogueData != null;
        public bool hasCommands => commandsData != null;

        public DialogueLine(string speaker, string dialogue, string commands)
        {
            this.speakerData = string.IsNullOrWhiteSpace(speaker) ? null : new DL_Speaker(speaker);
            this.dialogueData = string.IsNullOrWhiteSpace(dialogue) ? null : new DL_Text(dialogue);
            this.commandsData = string.IsNullOrWhiteSpace(commands) ? null : new DL_Commands(commands);
        }
    }
}

