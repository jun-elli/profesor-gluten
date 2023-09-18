using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dialogue.Characters
{
    /// <summary>
    /// Data container to define config params for a character
    /// </summary>

    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;
        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;

        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;
            result.nameFont = nameFont;
            result.dialogueFont = dialogueFont;

            // result.nameColor = nameColor - would reference the color
            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            return result;
        }

        private static Color _defaultTextColor => DialogueSystem.Instance.Config.defaultTextColor;
        private static TMP_FontAsset _defaultTextFont => DialogueSystem.Instance.Config.defaultTextFont;
        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;
                result.nameFont = _defaultTextFont;
                result.dialogueFont = _defaultTextFont;

                // result.nameColor = nameColor - would reference the color
                result.nameColor = new Color(_defaultTextColor.r, _defaultTextColor.g, _defaultTextColor.b, _defaultTextColor.a);
                result.dialogueColor = new Color(_defaultTextColor.r, _defaultTextColor.g, _defaultTextColor.b, _defaultTextColor.a);

                return result;
            }
        }
    }
}
