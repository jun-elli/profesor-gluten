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
        public RectTransform rootTransform = null;
        public Animator animator;
        public CharacterConfigData config;

        // Show and Hide fields
        protected Coroutine co_revealing, co_hiding;
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isVisible => false;

        protected CharacterManager manager => CharacterManager.Instance;
        private DialogueSystem _dialogueSystem => DialogueSystem.Instance;

        public enum CharacterType { Text, Sprite, SpriteSheet, Live2D, Model3D }

        // Constructor
        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;
            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, CharacterManager.Instance.CharacterPanel);
                ob.SetActive(true);
                rootTransform = ob.GetComponent<RectTransform>();
                animator = rootTransform.GetComponentInChildren<Animator>();
            }
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

        // Set config
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void SetNameColor(Color color) => config.nameColor = color;

        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;

        public void UpdateTextCostumizationOnScreen() => _dialogueSystem.ApplySpeakerDataToDialogueContainer(config);
        public void ResetConfigurationData() => config = CharacterManager.Instance.GetCharacterConfig(name);


        // Show and Hide methods
        public virtual Coroutine Show()
        {
            if (isRevealing)
            {
                return co_revealing;
            }
            if (isHiding)
            {
                manager.StopCoroutine(co_hiding);
            }
            return co_revealing = manager.StartCoroutine(ShowOrHide(true));
        }

        public virtual Coroutine Hide()
        {
            if (isHiding)
            {
                return co_hiding;
            }
            if (isRevealing)
            {
                manager.StopCoroutine(co_revealing);
            }
            return co_hiding = manager.StartCoroutine(ShowOrHide(false));
        }

        public virtual IEnumerator ShowOrHide(bool show)
        {
            yield return null;
        }

    }
}