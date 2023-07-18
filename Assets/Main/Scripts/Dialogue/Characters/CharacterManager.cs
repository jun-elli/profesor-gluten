using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public Character CreateCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogError($"Character {characterName} already exists.");
                return null;
            }

            return null;
        }
    }
}