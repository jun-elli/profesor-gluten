using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogueLine
    {
        public string speaker;
        public DL_Text dialogue;
        public string commands;

        public bool hasSpeaker => speaker != string.Empty;
        public bool hasDialogue => dialogue.hasDialogue;
        public bool hasCommands => commands != string.Empty;

        public DialogueLine(string speaker, string dialogue, string commands)
        {
            this.speaker = speaker;
            this.dialogue = new DL_Text(dialogue);
            this.commands = commands;
        }
    }
}

