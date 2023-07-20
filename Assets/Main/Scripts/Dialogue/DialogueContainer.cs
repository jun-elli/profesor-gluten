using UnityEngine;
using TMPro;
using Dialogue.Characters;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public TextMeshProUGUI dialogueText;
        public NameContainer nameContainer;

        void SetDialogueColor(Color color) => dialogueText.color = color;
        void SetDialogueFont(TMP_FontAsset font) => dialogueText.font = font;

        public void SetConfig(CharacterConfigData config)
        {
            SetDialogueColor(config.dialogueColor);
            SetDialogueFont(config.dialogueFont);

            nameContainer.SetConfig(config);
        }

    }
}
