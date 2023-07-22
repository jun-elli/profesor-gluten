using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Characters
{
    /// <summary>
    /// Manager to create, store in dict and retrieve Characters
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.Instance.Config.characterConfigurationAsset;

        // To retrieve prefab
        private const string CharacterNameID = "<characterName>";
        private string _characterRootPath => $"Characters/{CharacterNameID}";
        private string _characterPrefabPath => $"{_characterRootPath}/{CharacterNameID}";

        [SerializeField] private RectTransform _characterPanel = null;
        public RectTransform CharacterPanel => _characterPanel;

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
            // Check if it already exists in manager dictionary
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogError($"Character {characterName} already exists.");
                return null;
            }
            // Get character config data
            CharacterInfo info = GetCharacterInfo(characterName);
            // Create character with data
            Character character = CreateChatacterFromInfo(info);
            // Add to dict
            characters.Add(characterName.ToLower(), character);

            return character;
        }

        // Get name and config data of a character by getting it from
        // the CharacterConfigSO asset we have in the DialogueSystemConfigSO
        private CharacterInfo GetCharacterInfo(string name)
        {
            CharacterInfo result = new CharacterInfo();
            result.name = name;
            result.config = config.GetConfig(name);
            result.prefab = GetPrefabForCharacter(name);

            return result;
        }

        private GameObject GetPrefabForCharacter(string name)
        {
            string prefabPath = FormatPrefabPath(_characterPrefabPath, name);
            return Resources.Load<GameObject>(prefabPath);
        }

        // We inject the character name to the path to get the correct direction
        private string FormatPrefabPath(string path, string name) => path.Replace(CharacterNameID, name);

        private Character CreateChatacterFromInfo(CharacterInfo info)
        {
            CharacterConfigData config = info.config;

            switch (config.characterType)
            {
                case Character.CharacterType.Text:
                    return new CharacterText(info.name, config);
                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new CharacterSprite(info.name, config, info.prefab);
                case Character.CharacterType.Live2D:
                    return new CharacterLive2D(info.name, config, info.prefab);
                case Character.CharacterType.Model3D:
                    return new CharacterModel3D(info.name, config, info.prefab);
                default:
                    Debug.LogError("Wrong character type. Can't create new character.");
                    return null;
            }
        }

        public Character GetCharacter(string characterName, bool shouldCreateIfDoesNotExist = false)
        {
            string lowName = characterName.ToLower();
            if (characters.ContainsKey(lowName))
            {
                return characters[lowName];
            }
            else if (shouldCreateIfDoesNotExist)
            {
                return CreateCharacter(lowName);
            }

            return null;
        }

        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        // Class to store character data: name and config
        private class CharacterInfo
        {
            public string name = "";
            public CharacterConfigData config = null;
            public GameObject prefab = null;
        }
    }
}