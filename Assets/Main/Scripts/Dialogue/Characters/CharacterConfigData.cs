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
    }
}
