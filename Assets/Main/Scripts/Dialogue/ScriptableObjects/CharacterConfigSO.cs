using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    [CreateAssetMenu(fileName = "Character Sample", menuName = "Dialogue System/ Character Configuration Asset")]
    public class CharacterConfigSO : ScriptableObject
    {
        public CharacterConfigData[] characters;
    }
}