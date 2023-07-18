using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    /// <summary>
    /// SO that stores an array of all CharacterConfigData we have and a function to retrieve
    /// config data if we have it. If not, returns default values.
    /// </summary>
    [CreateAssetMenu(fileName = "Character Sample", menuName = "Dialogue System/ Character Configuration Asset")]
    public class CharacterConfigSO : ScriptableObject
    {
        public CharacterConfigData[] characters;

        // Find Configuration Data for a specific character (if there is one)
        // We have to return a copy of it, so orginal data is not modified permanently if later latered in editor
        public CharacterConfigData GetConfig(string characterName)
        {
            characterName = characterName.ToLower();

            for (int i = 0; i < characters.Length; i++)
            {
                CharacterConfigData data = characters[i];
                if (string.Equals(characterName, data.name.ToLower()) || string.Equals(characterName, data.alias.ToLower()))
                {
                    return data.Copy();
                }
            }
            // If we don't find it, we return defaut values
            return CharacterConfigData.Default;
        }
    }
}