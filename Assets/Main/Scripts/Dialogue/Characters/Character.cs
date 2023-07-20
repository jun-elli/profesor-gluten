using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dialogue.Characters
{
    public abstract class Character
    {
        public string name = "";
        public string displayName = "";
        public RectTransform transform = null;
        public CharacterConfigData config;

        private DialogueSystem _dialogueSystem => DialogueSystem.Instance;

        public enum CharacterType { Text, Sprite, SpriteSheet, Live2D, Model3D }

        // Constructor
        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            displayName = name;
            this.config = config;
        }

        public Coroutine Say(string line) => Say(new List<string> { line });

        public Coroutine Say(List<string> dialogue)
        {
            List<string> conversation = new List<string>();

            foreach (string line in dialogue)
            {
                conversation.Add($"{displayName} \"{line}\"");
            }
            _dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCostumizationOnScreen();
            return _dialogueSystem.Say(conversation);
        }

        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void SetNameColor(Color color) => config.nameColor = color;

        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;

        public void UpdateTextCostumizationOnScreen() => _dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
        public void ResetConfigurationData() => config = CharacterManager.Instance.GetCharacterConfig(name);
    }
}