using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public abstract class Character
    {
        public string name = "";
        public string displayName = "";
        public RectTransform transform = null;
        public CharacterConfigData config;

        private DialogueSystem _dialogueSystem => DialogueSystem.Instance;

        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            displayName = name;
            this.config = config;
        }

        public Coroutine Say(string line) => Say(new List<string> { line });
        // {
        //     return _dialogueSystem.Say(displayName, line);
        // }
        public Coroutine Say(List<string> dialogue)
        {
            List<string> conversation = new List<string>();

            foreach (string line in dialogue)
            {
                conversation.Add($"{displayName} \"{line}\"");
            }
            _dialogueSystem.ShowSpeakerName(displayName);
            _dialogueSystem.ApplySpeakerDataToDialogueContainer(name);
            return _dialogueSystem.Say(conversation);
        }

        public enum CharacterType { Text, Sprite, SpriteSheet, Live2D, Model3D }
    }
}