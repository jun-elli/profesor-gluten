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

        private DialogueSystem _dialogueSystem => DialogueSystem.Instance;

        public Character(string name)
        {
            this.name = name;
            displayName = name;
        }

        public Coroutine Say(string line) => Say(new List<string> { line });
        public Coroutine Say(List<string> dialogue)
        {
            _dialogueSystem.ShowSpeakerName(displayName);
            return _dialogueSystem.Say(dialogue);
        }

        public enum CharacterType { Text, Sprite, SpriteSheet, Live2D, Model3D }
    }
}