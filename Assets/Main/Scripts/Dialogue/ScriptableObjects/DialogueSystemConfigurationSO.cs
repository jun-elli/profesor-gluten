using Dialogue.Characters;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {
        public CharacterConfigSO characterConfigurationAsset;
    }
}