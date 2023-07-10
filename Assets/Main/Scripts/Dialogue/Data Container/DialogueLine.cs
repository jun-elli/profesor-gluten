using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogueLine
    {
        public DL_Speaker speaker;
        public DL_Text dialogue;
        public string commands;

        public bool hasSpeaker => speaker != null;
        public bool hasDialogue => dialogue.hasDialogue;
        public bool hasCommands => commands != string.Empty;

        public DialogueLine(string speaker, string dialogue, string commands)
        {
            this.speaker = string.IsNullOrWhiteSpace(speaker) ? null : new DL_Speaker(speaker);
            this.dialogue = new DL_Text(dialogue);
            this.commands = commands;
        }
    }
}

