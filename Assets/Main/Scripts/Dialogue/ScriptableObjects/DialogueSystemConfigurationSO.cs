using Dialogue.Characters;
using UnityEngine;
using TMPro;

namespace Dialogue
{
    /// <summary>
    /// SO that contains a reference to the CharacterConfigSO and default values for fonts and color
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigurationAsset;

        public Color defaultTextColor = Color.black;
        public TMP_FontAsset defaultTextFont;
    }
}